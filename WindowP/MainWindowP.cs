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
        private string _linuxGitPull = "git pull";
        private string _linuxGitReset = "git reset --hard HEAD";
        private string _linuxGitRev = "git symbolic-ref --short -q HEAD || git rev-parse --short HEAD";
        private string _linuxGitCheckout = "git checkout";
        private string _linuxCommand = "zig build run-dpsv & zig build run-gamesv";
        private string _linuxZigUsing = "zvm use rr";

        public MainWindowP(IMainWindow window)
        {
            _mainFormV = window;
            _mainFormV.OpeningSettings += OnOpenSettings;
            _mainFormV.Launching += OnLaunchClient;
            _mainFormV.WindowClosed += OnClosed;
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
                WindowStyle = ProcessWindowStyle.Minimized,
                Verb = "runas"
            };

            Process process = Process.Start(checkCommitInfo);
            if (process == null) return string.Empty;
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.TrimEnd();
        }

        private void SetCommitOnServer(string newCommit)
        {
            string serverPath = App.GetCurrentProfile().Patches.ServerPatch;

            ProcessStartInfo checkCommitInfo = new ProcessStartInfo()
            {
                FileName = "wsl.exe",
                WorkingDirectory = serverPath,
                Arguments = $"--cd \"{serverPath}\"-- bash -c \"{_linuxGitReset} && {_linuxGitCheckout} {newCommit}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Minimized,
                Verb = "runas"
            };

            Process process = Process.Start(checkCommitInfo);
            if (process == null) return;
            process.WaitForExit();
        }

        private void PullRepository()
        {
            string serverPath = App.GetCurrentProfile().Patches.ServerPatch;

            ProcessStartInfo checkCommitInfo = new ProcessStartInfo()
            {
                FileName = "wsl.exe",
                WorkingDirectory = serverPath,
                Arguments = $"--cd \"{serverPath}\"-- bash -c \"{_linuxGitReset} && {_linuxGitPull}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Minimized,
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

            if (commitOnProfile == "master" || commitOnProfile == "prod")
            {
                PullRepository();
            }

            ProcessStartInfo startServerInfo = new ProcessStartInfo()
            {
                FileName = "wsl.exe",
                WorkingDirectory = folderPath,
                Arguments = $"--cd \"{folderPath}\"-- bash -i -c \"{_linuxZigUsing} & {_linuxCommand}\"",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Minimized,
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
                WindowStyle = ProcessWindowStyle.Minimized,
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

        private void KillAll()
        {
            KillProcess(ref _serverProcess);
            KillProcess(ref _hoyoProcess);
            KillProcess(ref _kcpshimProcess);
        }

        private void OnOpenSettings()
        {
            if (_profileSelector == null || _profileSelector.IsDisposed)
            {
                _profileSelector = new();
            }

            _profileSelector.Owner = _mainFormV as MainWindow;
            _profileSelector.Show();
        }

        private void OnLaunchClient()
        {
            Profile profile = App.GetCurrentProfile();

            if (profile.Name == string.Empty)
            {
                MessageBox.Show("Профиль не выбран. Пожалуйста выберите профиль");
                return;
            }

            KillAll();
            RunServer(profile.Patches.ServerPatch);
            RunWinExe(ProfileSettingName.Hoyo, profile.Patches.HoyoPatch);
            RunWinExe(ProfileSettingName.Kcpshim, profile.Patches.KcpshimPatch);
            RunWinExe(ProfileSettingName.Client, profile.Patches.ClientPatch);
        }

        private void OnClosed()
        {
            KillAll();
            App.SaveSelectedProfile();
        }
    }
}
