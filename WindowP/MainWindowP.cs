using System.Diagnostics;
using System.IO;
using System.Windows;
using ZZZ_PS_Launcher.WindowI;
using ZZZ_PS_Launcher.WindowV;

namespace ZZZ_PS_Launcher.WindowP
{
    class MainWindowP
    {
        private static ProfileSelectorWindow _profileSelector;
        private IMainWindow _mainFormV;
        private Process _serverProcess;
        private Process _hoyoProcess;
        private Process _kcpshimProcess;
        private string _linuxGitRev = "git rev-parse --short HEAD";
        private string _linuxGitCheckout = "git checkout";
        private string _linuxCommand = "zig build run-dpsv & zig build run-gamesv";
        private string _linuxZigUsing = "zvm use rr";

        public MainWindowP(IMainWindow window)
        {
            _mainFormV = window;
            _mainFormV.OpeningSettings += OnOpenSettings;
            _mainFormV.LaunchingServer += OnLainchServer;
            _mainFormV.LaunchingClient += OnLaunchClient;


        }

        private string GetCommitOnServer()
        {
            string serverPath = App.GetCurrentProfile().Patches.ServerPatch;

            ProcessStartInfo checkCommitInfo = new ProcessStartInfo()
            {
                FileName = "wsl.exe",
                WorkingDirectory = serverPath,
                Arguments = $"--cd \"{serverPath}\"-- bash -c \"{_linuxGitRev}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Verb = "runas"
            };

            Process process = Process.Start(checkCommitInfo);
            if (process == null) return string.Empty;
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private void SetCommitOnServer(string newCommit)
        {
            string serverPath = App.GetCurrentProfile().Patches.ServerPatch;



            ProcessStartInfo checkCommitInfo = new ProcessStartInfo()
            {
                FileName = "wsl.exe",
                WorkingDirectory = serverPath,
                Arguments = $"--cd \"{serverPath}\"-- bash -c \"git reset --hard HEAD && {_linuxGitCheckout} {newCommit}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Verb = "runas"
            };

            Process process = Process.Start(checkCommitInfo);
            if (process == null) return;
            process.WaitForExit();
        }

        private void RunServer(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) return;

            string commitOnServer = GetCommitOnServer();
            string commitOnProfile = App.GetCurrentProfile().ServerCommit;

            if (commitOnProfile != commitOnServer)
            {
                SetCommitOnServer(commitOnProfile);
            }

            ProcessStartInfo startServerInfo = new ProcessStartInfo()
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
                _serverProcess = Process.Start(startServerInfo);
                ResetProcessField(ref _serverProcess);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show($"Ошибка wsl: {ex.Message}");
            }
        }

        private void RunWinExe(ProfileSettingName app, string fullPath)
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
                    case ProfileSettingName.Hoyo:
                        ResetProcessField(ref _hoyoProcess);
                        _hoyoProcess = process;
                        break;
                    case ProfileSettingName.Kcpshim:
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
            if (_profileSelector == null || _profileSelector.IsDisposed)
            {
                _profileSelector = new();
            }

            _profileSelector.Show();
        }

        private void OnLainchServer()
        {
            Profile profile = App.GetCurrentProfile();

            if (profile.Name == string.Empty)
            {
                MessageBox.Show("Профиль не выбран. Пожалуйста выберите профиль");
                return;
            }

            KillProcess(ref _serverProcess);
            KillProcess(ref _hoyoProcess);
            KillProcess(ref _kcpshimProcess);
            RunServer(profile.Patches.ServerPatch);
            RunWinExe(ProfileSettingName.Hoyo, profile.Patches.HoyoPatch);
            RunWinExe(ProfileSettingName.Kcpshim, profile.Patches.KcpshimPatch);
        }

        private void OnLaunchClient()
        {
            Profile profile = App.GetCurrentProfile();

            if (profile.Name == string.Empty)
            {
                MessageBox.Show("Профиль не выбран. Пожалуйста выберите профиль");
                return;
            }

            RunWinExe(ProfileSettingName.Client, profile.Patches.ClientPatch);
        }
    }
}
