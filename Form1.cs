using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Linq;

namespace NinjahZ_Tools
{
    public partial class Form1 : Form
    {
        FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
        private AppConfig config;
        private string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.cfg");

        public Form1()
        {
            InitializeComponent();
            config = AppConfig.Load(configFilePath);
            InitializeCheckBoxes();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                // Handle the event when a checkbox is checked or unchecked
                // You can customize the behavior here based on the checkbox that triggered the event
                if (checkBox.Checked)
                {
                    // Logic when the checkbox is checked
                    NoteLabel.Text = $"{checkBox.Text} is checked.";
                }
                else
                {
                    // Logic when the checkbox is unchecked
                    NoteLabel.Text = $"{checkBox.Text} is unchecked.";
                }
            }
        }

        private void InitializeCheckBoxes()
        {
            flowLayoutPanel.Dock = DockStyle.Fill; // Makes the panel fill the entire tab page
            flowLayoutPanel.AutoScroll = true;     // Adds a scrollbar if there are too many checkboxes
            flowLayoutPanel.FlowDirection = FlowDirection.TopDown; // Arrange controls from top to bottom
            tabPage1.Controls.Add(flowLayoutPanel);

            foreach (var entry in config.CheckBoxes)
            {
                CheckBox checkBox = new CheckBox
                {
                    Text = entry.Text,
                    AutoSize = true,
                };
                checkBox.CheckedChanged += CheckBox_CheckedChanged;
                flowLayoutPanel.Controls.Add(checkBox); // Add the CheckBox to the FlowLayoutPanel
            }
        }

        private async void DownloadButton_Click(object sender, EventArgs e)
        {
            List<Task> downloadTasks = new List<Task>();
            int checkedCount = 0;

            foreach (var checkBoxConfig in config.CheckBoxes)
            {
                var checkBox = flowLayoutPanel.Controls.OfType<CheckBox>().FirstOrDefault(cb => cb.Text == checkBoxConfig.Text);

                if (checkBox != null)
                {
                    Debug.WriteLine($"CheckBox Found: {checkBox.Text}, Checked: {checkBox.Checked}");

                    if (checkBox.Checked)
                    {
                        checkedCount += checkBoxConfig.Urls.Count;

                        foreach (var url in checkBoxConfig.Urls)
                        {
                            downloadTasks.Add(DownloadFileAsync(checkBox, url));
                        }
                    }
                }
            }

            if (checkedCount > 0)
            {
                DownloadProgressBar.Maximum = checkedCount;
                DownloadProgressBar.Value = 0;

                try
                {
                    await Task.WhenAll(downloadTasks);
                    MessageBox.Show("Download completed!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No files selected for download.");
            }
        }

        private async Task DownloadFileAsync(CheckBox checkBox, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    var fileName = Path.GetFileName(new Uri(url).LocalPath);

                    // Remove "Download" from the checkbox text and replace spaces with underscores
                    var folderName = checkBox.Text.Replace("Download ", "").Trim().Replace(" ", "_");
                    var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var folderPath = Path.Combine(desktopPath, folderName);

                    Directory.CreateDirectory(folderPath);

                    var savePath = Path.Combine(folderPath, fileName);

                    Invoke(new Action(() =>
                    {
                        StatusLabel.Text = $"Downloading: {fileName}";
                        fileProgressBar.Value = 0;
                        fileProgressBar.Maximum = 100; // Percentage-based updates
                    }));

                    using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        var buffer = new byte[8192];
                        var totalBytesRead = 0L;
                        var bytesRead = 0;
                        var totalBytes = response.Content.Headers.ContentLength.GetValueOrDefault(-1);

                        if (totalBytes > 0)
                        {
                            Invoke(new Action(() => fileProgressBar.Maximum = (int)totalBytes));
                        }

                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                totalBytesRead += bytesRead;

                                // Update progress
                                if (totalBytes != -1)
                                {
                                    int progressPercentage = (int)((totalBytesRead * 100) / totalBytes);
                                    Invoke(new Action(() =>
                                    {
                                        fileProgressBar.Value = progressPercentage;
                                        StatusLabel.Text = $"Downloading: {fileName} - {progressPercentage}%";
                                    }));
                                }
                                else
                                {
                                    Invoke(new Action(() =>
                                    {
                                        fileProgressBar.Value = (int)totalBytesRead;
                                        StatusLabel.Text = $"Downloading: {fileName}";
                                    }));
                                }
                            }
                        }
                    }

                    Invoke(new Action(() =>
                    {
                        StatusLabel.Text = $"{fileName} download complete";
                        DownloadProgressBar.Value++;
                    }));
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    StatusLabel.Text = $"Error downloading {url}: {ex.Message}";
                    fileProgressBar.Value = 0;
                }));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NoteLabel.Text = string.Empty;

            string tempFilePath = Path.GetTempFileName();

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "dxdiag",
                Arguments = $"/t \"{tempFilePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
            }

            if (File.Exists(tempFilePath))
            {
                try
                {
                    TextBoxSystemInfo.Text = File.ReadAllText(tempFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading system information: {ex.Message}");
                }
                finally
                {
                    File.Delete(tempFilePath);
                }
            }
            else
            {
                MessageBox.Show("Failed to generate system information.");
            }
        }

        private void RemoveOneDriveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the path to the OneDrive uninstaller
                string oneDriveSetupPath = @"C:\Windows\SysWOW64\OneDriveSetup.exe"; // For 64-bit systems
                                                                                     // For 32-bit systems, use: @"C:\Windows\System32\OneDriveSetup.exe"

                // Define the arguments for uninstallation
                string arguments = "/uninstall";

                // Start the process with the uninstaller command
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = oneDriveSetupPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();

                    // Check the exit code to determine if the uninstallation was successful
                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show("Microsoft OneDrive has been uninstalled successfully.");
                    }
                    else
                    {
                        MessageBox.Show($"Uninstallation failed with exit code {process.ExitCode}. Error: {process.StandardError.ReadToEnd()}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to uninstall OneDrive: {ex.Message}");
            }
        }

        private void DisableCortanaButton_Click(object sender, EventArgs e)
        {
            try
            {
                // PowerShell command to disable Cortana
                string command = "powershell.exe";
                string arguments = "-Command \"Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\Windows Search' -Name 'AllowCortana' -Value 0 -Type DWord\"";

                // Start the process with the command and arguments
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    // Wait for the process to exit
                    process.WaitForExit();

                    // Check the exit code to determine if the operation was successful
                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show("Cortana has been disabled successfully.");
                    }
                    else
                    {
                        MessageBox.Show($"Failed to disable Cortana. Error: {process.StandardError.ReadToEnd()}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to disable Cortana: {ex.Message}");
            }
        }

        private void DarkModeButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Path to the registry key for Dark Mode
                string registryPath = @"HKCU:\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
                string registryValueName = "AppsUseLightTheme";

                // Command to set the registry key value for Dark Mode
                string command = "powershell.exe";
                string arguments = $"-Command \"Set-ItemProperty -Path '{registryPath}' -Name '{registryValueName}' -Value 0\"";

                // Start the process with the command and arguments
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    // Wait for the process to exit
                    process.WaitForExit();

                    // Check the exit code to determine if the operation was successful
                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show("Dark Mode has been enabled.");
                    }
                    else
                    {
                        MessageBox.Show($"Failed to enable Dark Mode. Error: {process.StandardError.ReadToEnd()}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to enable Dark Mode: {ex.Message}");
            }
        }
    }
    public class AppConfig
    {
        public List<CheckBoxConfig> CheckBoxes { get; set; }

        public static AppConfig Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // Create default configuration if the file does not exist
                var defaultConfig = new AppConfig
                {
                    CheckBoxes = new List<CheckBoxConfig>
                    {
                        new CheckBoxConfig { Text = "X570F", Urls = new List<string>
                        {
                            "https://dlcdnets.asus.com/pub/ASUS/mb/04LAN/DRV_LAN_Intel_I211_SZ_TSD_W10_64_V121897_20200717R.zip?model=ROG%20STRIX%20X570-F%20GAMING",
                            "https://dlcdnets.asus.com/pub/ASUS/mb/Socket%20AM5/PRIME%20X670-P/DRV_Chipset_AMD_AM5_SZ-TSD_W11_64_V60516221_20240617R.zip?model=ROG%20STRIX%20X570-F%20GAMING",
                            "https://dlcdnets.asus.com/pub/ASUS/mb/01AUDIO/DRV_Audio_SZ_RTK_NS_6_W11_64_V6094111_20220928R.zip?model=ROG%20STRIX%20X570-F%20GAMING",
                            "https://dlcdnets.asus.com/pub/ASUS/mb/SocketAM4/ROG_CROSSHAIR_VIII_DARK_HERO/AISuite3_DIP_SystemInformation_EzUpdate_v3.03.36.zip?model=ROG%20STRIX%20X570-F%20GAMING",
                            "https://dlcdnets.asus.com/pub/ASUS/mb/14Utilities/ArmouryCrateInstallTool.zip?model=ROG%20STRIX%20X570-F%20GAMING",
                            "https://dlcdnets.asus.com/pub/ASUS/mb/BIOS/ROG-STRIX-X570-F-GAMING-ASUS-5013.zip?model=ROG%20STRIX%20X570-F%20GAMING"
                        }
                    },
                    new CheckBoxConfig { Text = "X570E", Urls = new List<string>
                    {
                        "https://dlcdnets.asus.com/pub/ASUS/mb/04LAN/DRV_LAN_Realtek_8125_SZ-TSD_W11_64_V112596142022_20220908B.zip?model=ROG%20STRIX%20X570-E%20GAMING",
                        "https://dlcdnets.asus.com/pub/ASUS/mb/08WIRELESS/DRV_WiFi_Intel_All_SZ-TSD_W11_64_V2216003_20220909R.zip?model=ROG%20STRIX%20X570-E%20GAMING",
                        "https://dlcdnets.asus.com/pub/ASUS/mb/Socket%20AM5/PRIME%20X670-P/DRV_Chipset_AMD_AM5_SZ-TSD_W11_64_V60516221_20240617R.zip?model=ROG%20STRIX%20X570-E%20GAMING",
                        "https://dlcdnets.asus.com/pub/ASUS/mb/01AUDIO/DRV_Audio_SZ_RTK_NS_6_W11_64_V6094111_20220928R.zip?model=ROG%20STRIX%20X570-E%20GAMING",
                        "https://dlcdnets.asus.com/pub/ASUS/mb/02BT/DRV_Bluetooth_Intel_All_SZ-TSD_W11_64_V2216003_20220909R.zip?model=ROG%20STRIX%20X570-E%20GAMING",
                        "https://dlcdnets.asus.com/pub/ASUS/mb/SocketAM4/ROG_CROSSHAIR_VIII_DARK_HERO/AISuite3_DIP_SystemInformation_EzUpdate_v3.03.36.zip?model=ROG%20STRIX%20X570-E%20GAMING",
                        "https://dlcdnets.asus.com/pub/ASUS/mb/14Utilities/ArmouryCrateInstallTool.zip?model=ROG%20STRIX%20X570-E%20GAMING",
                        "https://dlcdnets.asus.com/pub/ASUS/mb/BIOS/ROG-STRIX-X570-E-GAMING-ASUS-5013.zip?model=ROG%20STRIX%20X570-E%20GAMING"
                    }
                },
                new CheckBoxConfig { Text = "STEAM", Urls = new List<string> { "https://cdn.akamai.steamstatic.com/client/installer/SteamSetup.exe" } },
                new CheckBoxConfig { Text = "EPIC", Urls = new List<string> { "https://epicgames-download1.akamaized.net/Builds/UnrealEngineLauncher/Installers/Win32/EpicInstaller-15.17.1.msi?launcherfilename=EpicInstaller-15.17.1-c5d141533b9c4748941fa8d9453c2b27.msi" } },
                new CheckBoxConfig { Text = "CHROME", Urls = new List<string> { "https://dl.google.com/tag/s/appguid%3D%7B8A69D345-D564-463C-AFF1-A69D9E530F96%7D%26iid%3D%7B67D8FA1E-7803-716D-D81E-91E299A48F9F%7D%26lang%3Den-GB%26browser%3D4%26usagestats%3D0%26appname%3DGoogle%2520Chrome%26needsadmin%3Dprefers%26ap%3Dx64-statsdef_1%26brand%3DYTUH%26installdataindex%3Dempty/update2/installers/ChromeSetup.exe" } },
                new CheckBoxConfig { Text = "WINRAR", Urls = new List<string> { "https://www.win-rar.com/fileadmin/winrar-versions/winrar/winrar-x64-701.exe" } },
                new CheckBoxConfig { Text = "SAMSUNG", Urls = new List<string> { "https://download.semiconductor.samsung.com/resources/software-resources/Samsung_Magician_Installer_Official_8.1.0.800.exe" } },
                new CheckBoxConfig { Text = "CPUZ", Urls = new List<string> { "https://download.cpuid.com/cpu-z/cpu-z_2.10-rog-en.exe" } },
                new CheckBoxConfig { Text = "GPUZ", Urls = new List<string> { "https://sg1-dl.techpowerup.com/files/25im0t8xkLEdCYL5Y_BS-w/1722284827/GPU-Z_ASUS_ROG_2.59.0.exe" } },
                new CheckBoxConfig { Text = "CORETEMP", Urls = new List<string> { "https://www.alcpu.com/CoreTemp/Core-Temp-setup-v1.18.1.0.exe" } },
                new CheckBoxConfig { Text = "RYZENMASTER", Urls = new List<string> { "https://download.amd.com/Desktop/amd-ryzen-master.exe" } },
                new CheckBoxConfig { Text = "4060DRIVER", Urls = new List<string> { "https://download.gigabyte.com/FileList/Driver/551.86-desktop-win10-win11-64bit-international-dch-whql.exe?v=c9e5e21d3b1bbda38f85f5d64c148835" } },
                new CheckBoxConfig { Text = "MINECRAFT", Urls = new List<string> { "https://launcher.mojang.com/download/MinecraftInstaller.msi?ref=mcnet" } },
                new CheckBoxConfig { Text = "VISUALSTUDIO", Urls = new List<string> { "https://aka.ms/vs/17/release/vs_community.exe" } },
            }
                };
                Save(filePath, defaultConfig);
                return defaultConfig;
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<AppConfig>(json);
        }

        public class CheckBoxConfig
        {
            public string Text { get; set; }
            public List<string> Urls { get; set; }
        }

        public static void Save(string filePath, AppConfig config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
