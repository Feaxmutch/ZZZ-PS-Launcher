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
            openSettings_button = new Button();
            launchServer_button = new Button();
            SuspendLayout();
            // 
            // openSettings_button
            // 
            openSettings_button.Location = new Point(26, 26);
            openSettings_button.Name = "openSettings_button";
            openSettings_button.Size = new Size(75, 23);
            openSettings_button.TabIndex = 0;
            openSettings_button.Text = "Настройки";
            openSettings_button.UseVisualStyleBackColor = true;
            openSettings_button.Click += openSettings_button_Click;
            // 
            // launchServer_button
            // 
            launchServer_button.Location = new Point(244, 224);
            launchServer_button.Name = "launchServer_button";
            launchServer_button.Size = new Size(75, 23);
            launchServer_button.TabIndex = 1;
            launchServer_button.Text = "Запуск";
            launchServer_button.UseVisualStyleBackColor = true;
            launchServer_button.Click += launchServer_button_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 321);
            Controls.Add(launchServer_button);
            Controls.Add(openSettings_button);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "ZZZ PS Launcher";
            ResumeLayout(false);
        }

        #endregion

        private Button openSettings_button;
        private Button launchServer_button;
    }
}