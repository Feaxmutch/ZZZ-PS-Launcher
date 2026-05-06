using System.Drawing.Drawing2D;

namespace ZZZ_PS_Launcher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            openSettings_button = new PictureBox();
            launchServer_button = new PictureBox();
            launchServer_label = new Label();
            launchClient_label = new Label();
            launchClient_button = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)openSettings_button).BeginInit();
            ((System.ComponentModel.ISupportInitialize)launchServer_button).BeginInit();
            ((System.ComponentModel.ISupportInitialize)launchClient_button).BeginInit();
            SuspendLayout();
            // 
            // openSettings_button
            // 
            openSettings_button.BackColor = Color.Transparent;
            openSettings_button.BackgroundImage = Properties.Resources.Settings_button;
            openSettings_button.BackgroundImageLayout = ImageLayout.Zoom;
            openSettings_button.Location = new Point(785, 12);
            openSettings_button.Name = "openSettings_button";
            openSettings_button.Size = new Size(51, 47);
            openSettings_button.TabIndex = 4;
            openSettings_button.TabStop = false;
            openSettings_button.Click += openSettings_button_Click;
            // 
            // launchServer_button
            // 
            launchServer_button.BackColor = Color.Transparent;
            launchServer_button.BackgroundImage = Properties.Resources.PlayButton;
            launchServer_button.BackgroundImageLayout = ImageLayout.Zoom;
            launchServer_button.Location = new Point(632, 406);
            launchServer_button.Name = "launchServer_button";
            launchServer_button.Size = new Size(194, 60);
            launchServer_button.TabIndex = 5;
            launchServer_button.TabStop = false;
            launchServer_button.Click += launchServer_button_Click;
            // 
            // launchServer_label
            // 
            launchServer_label.AutoSize = true;
            launchServer_label.BackColor = Color.Gold;
            launchServer_label.Font = new Font("Segoe UI Black", 10F);
            launchServer_label.Location = new Point(673, 426);
            launchServer_label.Name = "launchServer_label";
            launchServer_label.Size = new Size(134, 19);
            launchServer_label.TabIndex = 6;
            launchServer_label.Text = "Запустить сервер";
            launchServer_label.Click += launchServer_button_Click;
            // 
            // launchClient_label
            // 
            launchClient_label.AutoSize = true;
            launchClient_label.BackColor = Color.Gold;
            launchClient_label.Font = new Font("Segoe UI Black", 10F);
            launchClient_label.Location = new Point(673, 371);
            launchClient_label.Name = "launchClient_label";
            launchClient_label.Size = new Size(134, 19);
            launchClient_label.TabIndex = 8;
            launchClient_label.Text = "Запустить клиент";
            launchClient_label.Click += launchClient_button_Click;
            // 
            // launchClient_button
            // 
            launchClient_button.BackColor = Color.Transparent;
            launchClient_button.BackgroundImage = Properties.Resources.PlayButton;
            launchClient_button.BackgroundImageLayout = ImageLayout.Zoom;
            launchClient_button.Location = new Point(632, 351);
            launchClient_button.Name = "launchClient_button";
            launchClient_button.Size = new Size(194, 60);
            launchClient_button.TabIndex = 7;
            launchClient_button.TabStop = false;
            launchClient_button.Click += launchClient_button_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.MainBackground;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(848, 477);
            Controls.Add(launchClient_label);
            Controls.Add(launchClient_button);
            Controls.Add(launchServer_label);
            Controls.Add(launchServer_button);
            Controls.Add(openSettings_button);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ZZZ PS Launcher";
            ((System.ComponentModel.ISupportInitialize)openSettings_button).EndInit();
            ((System.ComponentModel.ISupportInitialize)launchServer_button).EndInit();
            ((System.ComponentModel.ISupportInitialize)launchClient_button).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox openSettings_button;
        private PictureBox launchServer_button;
        private Label launchServer_label;
        private Label launchClient_label;
        private PictureBox launchClient_button;
    }
}