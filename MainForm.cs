
using Microsoft.Win32;
using System.Diagnostics;

namespace ZZZ_PS_Launcher
{
    public partial class MainForm : Form
    {
        private SettingsForm _settingsForm;

        private string _serverPath = string.Empty;
        private string _hoyoPath = string.Empty;
        private string _kcpshimPath = string.Empty;
        private string _clientPath = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadSettings()
        {
            _serverPath = (string)Registry.GetValue(Program.SettingsPath, PathNames.ServerPath, string.Empty);
            _kcpshimPath = (string)Registry.GetValue(Program.SettingsPath, PathNames.KcpshimPath, string.Empty);
            _hoyoPath = (string)Registry.GetValue(Program.SettingsPath, PathNames.HoyoSdkPath, string.Empty);
            _clientPath = (string)Registry.GetValue(Program.SettingsPath, PathNames.ClientPath, string.Empty);
        }

        private bool PathesIsValide()
        {
            string messageStart = "Указан неверный путь к";

            if ((Directory.Exists(_serverPath + "\\dpsv") && Directory.Exists(_serverPath + "\\gamesv")) == false)
            {
                MessageBox.Show($"{messageStart} серверу: {_serverPath}");
                return false;
            }

            if (File.Exists(_kcpshimPath) == false)
            {
                MessageBox.Show($"{messageStart} kcpshim: {_kcpshimPath}");
                return false;
            }

            if (File.Exists(_hoyoPath) == false)
            {
                MessageBox.Show($"{messageStart} hoyo-sdk: {_hoyoPath}");
                return false;
            }

            if (File.Exists(_clientPath) == false)
            {
                MessageBox.Show($"{messageStart} yidhari: {_clientPath}");
                return false;
            }

            return true;
        }

        private void RunServer(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) return;
            string linuxCommands = "zig build run-dpsv & zig build run-gamesv; exec bash";

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                WorkingDirectory = folderPath,
                Arguments = $"-NoExit -Command \"wsl --cd '{folderPath}' -u root -- bash -i -c '{linuxCommands}'\"",
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process.Start(psi);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode != 1223) MessageBox.Show($"Ошибка PowerShell: {ex.Message}");
            }
        }

        private void RunWinExe(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath)) return;

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = fullPath,
                WorkingDirectory = Path.GetDirectoryName(fullPath),
                UseShellExecute = true,
                Verb = "runas"
            };

            try { Process.Start(psi); }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode != 1223)
                {
                    MessageBox.Show($"Ошибка системы при запуске {Path.GetFileName(fullPath)}: {ex.Message}");
                }
            }
        }

        private void openSettings_button_Click(object sender, EventArgs e)
        {
            if (_settingsForm == null || _settingsForm.IsDisposed)
            {
                _settingsForm = new();
            }

            _settingsForm.Show();
        }

        private void launchServer_button_Click(object sender, EventArgs e)
        {
            LoadSettings();

            if (PathesIsValide())
            {
                RunServer(_serverPath);
                RunWinExe(_hoyoPath);
                RunWinExe(_kcpshimPath);
                RunWinExe(_clientPath);
            }
        }
    }
}
