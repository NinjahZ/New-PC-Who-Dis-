﻿namespace NinjahZ_Tools
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DownloadButton = new System.Windows.Forms.Button();
            this.DownloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.fileProgressBar = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TextBoxSystemInfo = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.RemoveOneDriveButton = new System.Windows.Forms.Button();
            this.DisableCortanaButton = new System.Windows.Forms.Button();
            this.DarkModeButton = new System.Windows.Forms.Button();
            this.NoteLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // DownloadButton
            // 
            this.DownloadButton.Location = new System.Drawing.Point(6, 473);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(534, 23);
            this.DownloadButton.TabIndex = 14;
            this.DownloadButton.Text = "Download";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // DownloadProgressBar
            // 
            this.DownloadProgressBar.Location = new System.Drawing.Point(6, 526);
            this.DownloadProgressBar.Name = "DownloadProgressBar";
            this.DownloadProgressBar.Size = new System.Drawing.Size(534, 18);
            this.DownloadProgressBar.TabIndex = 15;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(8, 452);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(47, 13);
            this.StatusLabel.TabIndex = 17;
            this.StatusLabel.Text = "Awaiting";
            // 
            // fileProgressBar
            // 
            this.fileProgressBar.Location = new System.Drawing.Point(6, 502);
            this.fileProgressBar.Name = "fileProgressBar";
            this.fileProgressBar.Size = new System.Drawing.Size(534, 18);
            this.fileProgressBar.TabIndex = 18;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(559, 577);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DownloadProgressBar);
            this.tabPage1.Controls.Add(this.fileProgressBar);
            this.tabPage1.Controls.Add(this.StatusLabel);
            this.tabPage1.Controls.Add(this.DownloadButton);
            this.tabPage1.Controls.Add(this.NoteLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(551, 551);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Downloads";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TextBoxSystemInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(551, 551);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "System Information";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TextBoxSystemInfo
            // 
            this.TextBoxSystemInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxSystemInfo.Location = new System.Drawing.Point(3, 3);
            this.TextBoxSystemInfo.Multiline = true;
            this.TextBoxSystemInfo.Name = "TextBoxSystemInfo";
            this.TextBoxSystemInfo.ReadOnly = true;
            this.TextBoxSystemInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxSystemInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxSystemInfo.Size = new System.Drawing.Size(545, 545);
            this.TextBoxSystemInfo.TabIndex = 0;
            this.TextBoxSystemInfo.TabStop = false;
            this.TextBoxSystemInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.DarkModeButton);
            this.tabPage3.Controls.Add(this.DisableCortanaButton);
            this.tabPage3.Controls.Add(this.RemoveOneDriveButton);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(551, 551);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tools";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // RemoveOneDriveButton
            // 
            this.RemoveOneDriveButton.Location = new System.Drawing.Point(3, 3);
            this.RemoveOneDriveButton.Name = "RemoveOneDriveButton";
            this.RemoveOneDriveButton.Size = new System.Drawing.Size(109, 23);
            this.RemoveOneDriveButton.TabIndex = 0;
            this.RemoveOneDriveButton.Text = "Remove OneDrive";
            this.RemoveOneDriveButton.UseVisualStyleBackColor = true;
            this.RemoveOneDriveButton.Click += new System.EventHandler(this.RemoveOneDriveButton_Click);
            // 
            // DisableCortanaButton
            // 
            this.DisableCortanaButton.Location = new System.Drawing.Point(3, 32);
            this.DisableCortanaButton.Name = "DisableCortanaButton";
            this.DisableCortanaButton.Size = new System.Drawing.Size(109, 23);
            this.DisableCortanaButton.TabIndex = 1;
            this.DisableCortanaButton.Text = "Disable Cortana";
            this.DisableCortanaButton.UseVisualStyleBackColor = true;
            this.DisableCortanaButton.Click += new System.EventHandler(this.DisableCortanaButton_Click);
            // 
            // DarkModeButton
            // 
            this.DarkModeButton.Location = new System.Drawing.Point(3, 61);
            this.DarkModeButton.Name = "DarkModeButton";
            this.DarkModeButton.Size = new System.Drawing.Size(109, 23);
            this.DarkModeButton.TabIndex = 2;
            this.DarkModeButton.Text = "Enable Dark Mode";
            this.DarkModeButton.UseVisualStyleBackColor = true;
            this.DarkModeButton.Click += new System.EventHandler(this.DarkModeButton_Click);
            // 
            // NoteLabel
            // 
            this.NoteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoteLabel.Location = new System.Drawing.Point(290, 3);
            this.NoteLabel.Name = "NoteLabel";
            this.NoteLabel.Size = new System.Drawing.Size(255, 24);
            this.NoteLabel.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 575);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New PC Who Dis?";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button DownloadButton;
        private System.Windows.Forms.ProgressBar DownloadProgressBar;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.ProgressBar fileProgressBar;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox TextBoxSystemInfo;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button RemoveOneDriveButton;
        private System.Windows.Forms.Button DisableCortanaButton;
        private System.Windows.Forms.Button DarkModeButton;
        private System.Windows.Forms.Label NoteLabel;
    }
}

