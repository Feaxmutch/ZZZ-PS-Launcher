namespace ZZZ_PS_Launcher
{
    partial class SettingsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            server_label = new Label();
            server_textBox = new TextBox();
            server_button = new Button();
            hoyo_button = new Button();
            hoyo_textBox = new TextBox();
            hoyo_label = new Label();
            kcpshim_button = new Button();
            kcpshim_textBox = new TextBox();
            kcpshim_label = new Label();
            client_button = new Button();
            client_textBox = new TextBox();
            client_label = new Label();
            saveSettings_button = new Button();
            SuspendLayout();
            // 
            // server_label
            // 
            server_label.AutoSize = true;
            server_label.Location = new Point(20, 20);
            server_label.Name = "server_label";
            server_label.Size = new Size(127, 15);
            server_label.TabIndex = 0;
            server_label.Text = "Путь к папке сервера:";
            // 
            // server_textBox
            // 
            server_textBox.Location = new Point(20, 45);
            server_textBox.Name = "server_textBox";
            server_textBox.Size = new Size(420, 23);
            server_textBox.TabIndex = 1;
            // 
            // server_button
            // 
            server_button.Location = new Point(450, 45);
            server_button.Name = "server_button";
            server_button.Size = new Size(110, 23);
            server_button.TabIndex = 2;
            server_button.Text = "Указать путь";
            server_button.UseVisualStyleBackColor = true;
            server_button.Click += server_button_Click;
            // 
            // hoyo_button
            // 
            hoyo_button.Location = new Point(450, 105);
            hoyo_button.Name = "hoyo_button";
            hoyo_button.Size = new Size(110, 23);
            hoyo_button.TabIndex = 5;
            hoyo_button.Text = "Указать путь";
            hoyo_button.UseVisualStyleBackColor = true;
            hoyo_button.Click += hoyo_button_Click;
            // 
            // hoyo_textBox
            // 
            hoyo_textBox.Location = new Point(20, 105);
            hoyo_textBox.Name = "hoyo_textBox";
            hoyo_textBox.Size = new Size(420, 23);
            hoyo_textBox.TabIndex = 4;
            // 
            // hoyo_label
            // 
            hoyo_label.AutoSize = true;
            hoyo_label.Location = new Point(20, 80);
            hoyo_label.Name = "hoyo_label";
            hoyo_label.Size = new Size(100, 15);
            hoyo_label.TabIndex = 3;
            hoyo_label.Text = "Путь к Hoyo-sdk:";
            // 
            // kcpshim_button
            // 
            kcpshim_button.Location = new Point(450, 165);
            kcpshim_button.Name = "kcpshim_button";
            kcpshim_button.Size = new Size(110, 23);
            kcpshim_button.TabIndex = 8;
            kcpshim_button.Text = "Указать путь";
            kcpshim_button.UseVisualStyleBackColor = true;
            kcpshim_button.Click += kcpshim_button_Click;
            // 
            // kcpshim_textBox
            // 
            kcpshim_textBox.Location = new Point(20, 165);
            kcpshim_textBox.Name = "kcpshim_textBox";
            kcpshim_textBox.Size = new Size(420, 23);
            kcpshim_textBox.TabIndex = 7;
            // 
            // kcpshim_label
            // 
            kcpshim_label.AutoSize = true;
            kcpshim_label.Location = new Point(20, 140);
            kcpshim_label.Name = "kcpshim_label";
            kcpshim_label.Size = new Size(94, 15);
            kcpshim_label.TabIndex = 6;
            kcpshim_label.Text = "Путь к Kcpshim:";
            // 
            // client_button
            // 
            client_button.Location = new Point(450, 225);
            client_button.Name = "client_button";
            client_button.Size = new Size(110, 23);
            client_button.TabIndex = 11;
            client_button.Text = "Указать путь";
            client_button.UseVisualStyleBackColor = true;
            client_button.Click += client_button_Click;
            // 
            // client_textBox
            // 
            client_textBox.Location = new Point(20, 225);
            client_textBox.Name = "client_textBox";
            client_textBox.Size = new Size(420, 23);
            client_textBox.TabIndex = 10;
            // 
            // client_label
            // 
            client_label.AutoSize = true;
            client_label.Location = new Point(20, 200);
            client_label.Name = "client_label";
            client_label.Size = new Size(187, 15);
            client_label.TabIndex = 9;
            client_label.Text = "Путь к Yidhari (патчеру клиента):";
            // 
            // saveSettings_button
            // 
            saveSettings_button.Location = new Point(225, 266);
            saveSettings_button.Name = "saveSettings_button";
            saveSettings_button.Size = new Size(150, 43);
            saveSettings_button.TabIndex = 12;
            saveSettings_button.Text = "Сохранить";
            saveSettings_button.UseVisualStyleBackColor = true;
            saveSettings_button.Click += saveSettings_button_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(584, 321);
            Controls.Add(saveSettings_button);
            Controls.Add(client_button);
            Controls.Add(client_textBox);
            Controls.Add(client_label);
            Controls.Add(kcpshim_button);
            Controls.Add(kcpshim_textBox);
            Controls.Add(kcpshim_label);
            Controls.Add(hoyo_button);
            Controls.Add(hoyo_textBox);
            Controls.Add(hoyo_label);
            Controls.Add(server_button);
            Controls.Add(server_textBox);
            Controls.Add(server_label);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Настройки";
            FormClosing += SettingsForm_FormClosing;
            Load += SettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label server_label;
        private TextBox server_textBox;
        private Button server_button;
        private Button hoyo_button;
        private TextBox hoyo_textBox;
        private Label hoyo_label;
        private Button kcpshim_button;
        private TextBox kcpshim_textBox;
        private Label kcpshim_label;
        private Button client_button;
        private TextBox client_textBox;
        private Label client_label;
        private Button saveSettings_button;
    }
}
