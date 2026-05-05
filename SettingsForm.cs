using Microsoft.Win32;

namespace ZZZ_PS_Launcher
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SelectFolder(TextBox targetTextBox)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку с сервером";
                folderDialog.UseDescriptionForTitle = true;
                folderDialog.InitialDirectory = @"\\wsl.localhost";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    targetTextBox.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void SelectExeFile(TextBox targetTextBox)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Исполняемые файлы (*.exe)|*.exe";
                fileDialog.Title = "Выберите exe файл";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    targetTextBox.Text = fileDialog.FileName;
                }
            }
        }

        private void SaveSettings()
        {
            Registry.SetValue(Program.SettingsPath, PathNames.ServerPath, server_textBox.Text);
            Registry.SetValue(Program.SettingsPath, PathNames.KcpshimPath, kcpshim_textBox.Text);
            Registry.SetValue(Program.SettingsPath, PathNames.HoyoSdkPath, hoyo_textBox.Text);
            Registry.SetValue(Program.SettingsPath, PathNames.ClientPath, client_textBox.Text);
        }

        private void LoadSettings()
        {
            server_textBox.Text = (string)Registry.GetValue(Program.SettingsPath, PathNames.ServerPath, string.Empty);
            kcpshim_textBox.Text = (string)Registry.GetValue(Program.SettingsPath, PathNames.KcpshimPath, string.Empty);
            hoyo_textBox.Text = (string)Registry.GetValue(Program.SettingsPath, PathNames.HoyoSdkPath, string.Empty);
            client_textBox.Text = (string)Registry.GetValue(Program.SettingsPath, PathNames.ClientPath, string.Empty);
        }

        private void server_button_Click(object sender, EventArgs e)
        {
            SelectFolder(server_textBox);
        }

        private void hoyo_button_Click(object sender, EventArgs e)
        {
            SelectExeFile(hoyo_textBox);
        }

        private void kcpshim_button_Click(object sender, EventArgs e)
        {
            SelectExeFile(kcpshim_textBox);
        }

        private void client_button_Click(object sender, EventArgs e)
        {
            SelectExeFile(client_textBox);
        }

        private void saveSettings_button_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }
    }
}
