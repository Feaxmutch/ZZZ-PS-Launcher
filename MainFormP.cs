using System.Diagnostics;

namespace ZZZ_PS_Launcher
{
    internal class MainFormP
    {
        private IMainForm _mainFormV;
        private Process _serverProcess;
        private Process _hoyoProcess;
        private Process _kcpshimProcess;
        private string _linuxCommand = "zig build run-dpsv & zig build run-gamesv";
        private string _linuxZigUsing = "zvm use rr";

        public MainFormP(IMainForm mainForm)
        {
            _mainFormV = mainForm;
            _mainFormV.OpeningSettings += OnOpenSettings;
            _mainFormV.LaunchingServer += OnLainchServer;
            _mainFormV.LaunchingClient += OnLaunchClient;
        }

        private bool PathesIsValide()
        {
            string messageStart = "Указан неверный путь к";

            if ((Directory.Exists(Program.Patches.ServerPatch + "\\dpsv") && Directory.Exists(Program.Patches.ServerPatch + "\\gamesv")) == false)
            {
                MessageBox.Show($"{messageStart} серверу: {Program.Patches.ServerPatch}");
                return false;
            }

            if (File.Exists(Program.Patches.KcpshimPatch) == false)
            {
                MessageBox.Show($"{messageStart} kcpshim: {Program.Patches.KcpshimPatch}");
                return false;
            }

            if (File.Exists(Program.Patches.HoyoPatch) == false)
            {
                MessageBox.Show($"{messageStart} hoyo-sdk: {Program.Patches.HoyoPatch}");
                return false;
            }

            if (File.Exists(Program.Patches.ClientPatch) == false)
            {
                MessageBox.Show($"{messageStart} yidhari: {Program.Patches.ClientPatch}");
                return false;
            }

            return true;
        }

        private void RunServer(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) return;

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "wsl.exe",
                WorkingDirectory = folderPath,
                Arguments = $"--cd \"{folderPath}\"-- bash -i -c \"{_linuxZigUsing} & {_linuxCommand}\"",
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                ResetProcessField(ref _serverProcess);
                _serverProcess = Process.Start(psi);
                ResetProcessField(ref _serverProcess);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show($"Ошибка wsl: {ex.Message}");
            }
        }

        private void RunWinExe(App app, string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath)) return;

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = fullPath,
                WorkingDirectory = Path.GetDirectoryName(fullPath),
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process process = Process.Start(psi);

                switch (app)
                {
                    case App.Hoyo:
                        ResetProcessField(ref _hoyoProcess);
                        _hoyoProcess = process;
                        break;
                    case App.Kcpshim:
                        ResetProcessField(ref _kcpshimProcess);
                        _kcpshimProcess = process;
                        break;
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode != 1223)
                {
                    MessageBox.Show($"Ошибка системы при запуске {Path.GetFileName(fullPath)}: {ex.Message}");
                }
            }
        }

        private void ResetProcessField(ref Process process)
        {
            if (process != null)
            {
                process.Refresh();

                if (process.HasExited)
                {
                    process = null;
                }
            }
        }

        private void KillProcess(ref Process process)
        {
            ResetProcessField(ref process);

            if (process != null)
            {
                process.Kill(true);
                process.WaitForExit();
                ResetProcessField(ref process);
            }
        }

        private void OnOpenSettings()
        {
            Program.OpenSettings();
        }

        private void OnLainchServer()
        {
            Program.LoadSettings();

            if (PathesIsValide())
            {
                KillProcess(ref _serverProcess);
                KillProcess(ref _hoyoProcess);
                KillProcess(ref _kcpshimProcess);
                RunServer(Program.Patches.ServerPatch);
                RunWinExe(App.Hoyo, Program.Patches.HoyoPatch);
                RunWinExe(App.Kcpshim, Program.Patches.KcpshimPatch);
            }
        }

        private void OnLaunchClient()
        {
            Program.LoadSettings();

            if (PathesIsValide())
            {
                RunWinExe(App.Client, Program.Patches.ClientPatch);
            }
        }
    }
}
