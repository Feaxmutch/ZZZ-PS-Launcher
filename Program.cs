using Microsoft.Win32;
using System.Diagnostics;

namespace ZZZ_PS_Starter
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LauncherForm());
        }
    }

    public class PathNames
    {
        public string ServerPath => nameof(ServerPath);
        public string HoyoSdkPath => nameof(HoyoSdkPath);
        public string KcpshimPath => nameof(KcpshimPath);
        public string ClientPath => nameof(ClientPath);
    }

    public class LauncherForm : Form
    {
        private TextBox _txtYoshunkoPath;
        private TextBox _txtKcpshimPath;
        private TextBox _txtHoyoSdkPath;
        private TextBox _txtYidhariPath;
        private CheckBox _noClient;

        private PathNames _pathNames = new PathNames();
        private string _settingsPath = @"HKEY_CURRENT_USER\Software\ZZZServerStarter";
        private int _lableSpace = 25;
        private int _textBoxSpace = 35;
        private Size _windowSize = new Size(600, 360);

        public LauncherForm(string formName = "ZZZ Server Launcher")
        {
            Text = formName;
            Icon = Resources.Icon;
            Size = _windowSize;
            StartPosition = FormStartPosition.CenterScreen;
            int currentY = 20;

            AddSelector(ref currentY, ref _txtYoshunkoPath, true, "Путь к папке сервера:");
            AddSelector(ref currentY, ref _txtHoyoSdkPath, false, "Путь к Hoyo-sdk:");
            AddSelector(ref currentY, ref _txtKcpshimPath, false, "Путь к Kcpshim:");
            AddSelector(ref currentY, ref _txtYidhariPath, false, "Путь к Yidhari (патчеру клиента):");
            _noClient = CreateControl<CheckBox>(new Point(20, currentY + 5), 15);
            Label noClientDescriprion = CreateControl<Label>(new Point(35, currentY + 10), 200, "Не запускать клиент");
            Button btnLaunch = CreateControl<Button>(new Point(200, currentY), 200, "Запуск"); btnLaunch.Height = 35;
            btnLaunch.Click += OnLaunchClick;

            Controls.Add(btnLaunch);
            Controls.Add(_noClient);
            Controls.Add(noClientDescriprion);
            LoadSettings();
        }

        private T CreateControl<T>(Point position, int width, string text = "") where T : Control, new()
        {
            T control = new()
            {
                Location = position,
                Width = width
            };

            switch (control)
            {
                case Label:
                    (control as Label).Text = text;
                    break;

                case Button:
                    (control as Button).Text = text;
                    break;
            }

            return control;
        }

        private void AddSelector(ref int yPos, ref TextBox txtForSave, bool isFoldierSelector, string title)
        {
            string labelText = title;
            Label label = CreateControl<Label>(new Point(20, yPos), 400, labelText);
            yPos += _lableSpace;
            TextBox textBox = CreateControl<TextBox>(new Point(20, yPos), 420);
            Button button = CreateControl<Button>(new Point(450, yPos - 1), 110, "Указать путь");

            if (isFoldierSelector)
            {
                button.Click += (s, e) => SelectFolder(textBox);
            }
            else
            {
                button.Click += (s, e) => SelectExeFile(textBox);
            }

            yPos += _textBoxSpace;
            Controls.AddRange([label, textBox, button]);
            txtForSave = textBox;
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

        private void LoadSettings()
        {
            _txtYoshunkoPath.Text = (string)Registry.GetValue(_settingsPath, _pathNames.ServerPath, "");
            _txtKcpshimPath.Text = (string)Registry.GetValue(_settingsPath, _pathNames.KcpshimPath, "");
            _txtHoyoSdkPath.Text = (string)Registry.GetValue(_settingsPath, _pathNames.HoyoSdkPath, "");
            _txtYidhariPath.Text = (string)Registry.GetValue(_settingsPath, _pathNames.ClientPath, "");
            _noClient.Checked = Convert.ToBoolean((string)Registry.GetValue(_settingsPath, "NoClient", "False"));
        }

        private void SaveSettings()
        {
            Registry.SetValue(_settingsPath, _pathNames.ServerPath, _txtYoshunkoPath.Text);
            Registry.SetValue(_settingsPath, _pathNames.KcpshimPath, _txtKcpshimPath.Text);
            Registry.SetValue(_settingsPath, _pathNames.HoyoSdkPath, _txtHoyoSdkPath.Text);
            Registry.SetValue(_settingsPath, _pathNames.ClientPath, _txtYidhariPath.Text);
            Registry.SetValue(_settingsPath, "NoClient", _noClient.Checked);
        }

        private void OnLaunchClick(object sender, EventArgs e)
        {
            SaveSettings();

            if (PathesIsValide())
            {
                RunServer(_txtYoshunkoPath.Text);
                RunWinExe(_txtKcpshimPath.Text);
                RunWinExe(_txtHoyoSdkPath.Text);

                if (_noClient.Checked == false)
                {
                    RunWinExe(_txtYidhariPath.Text);
                }
            }
        }

        private bool PathesIsValide()
        {
            string messageStart = "Указан неверный путь к";

            if ((Directory.Exists(_txtYoshunkoPath.Text + "\\dpsv") && Directory.Exists(_txtYoshunkoPath.Text + "\\gamesv")) == false)
            {
                MessageBox.Show($"{messageStart} серверу: {_txtYoshunkoPath.Text}");
                return false;
            }

            if (File.Exists(_txtKcpshimPath.Text) == false)
            {
                MessageBox.Show($"{messageStart} kcpshim: {_txtKcpshimPath.Text}");
                return false;
            }

            if (File.Exists(_txtHoyoSdkPath.Text) == false)
            {
                MessageBox.Show($"{messageStart} hoyo-sdk: {_txtHoyoSdkPath.Text}");
                return false;
            }

            if (File.Exists(_txtYidhariPath.Text) == false)
            {
                MessageBox.Show($"{messageStart} Yidhari: {_txtYidhariPath.Text}");
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
    }
}