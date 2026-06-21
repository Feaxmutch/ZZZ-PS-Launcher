using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ZZZ_PS_Launcher
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

        private async Task<ProcessData> StartProcess(string fileName, string directory, string arguments, bool canOutput = false)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = fileName,
                WorkingDirectory = directory,
                Arguments = arguments,
                UseShellExecute = !canOutput,
                RedirectStandardOutput = canOutput,
                WindowStyle = ProcessWindowStyle.Minimized,
                Verb = "runas"
            };
            Process process = null;

            try
            {
                process = Process.Start(psi);

                if (process != null && canOutput)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return new ProcessData(process, output);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска процесса: {ex.Message}");
            }

            return new ProcessData(process, string.Empty);
        }

        private async Task<string> GetCommitOnServer()
        {
            string serverPath = App.GetCurrentProfile().Patches.ServerPatch;
            ProcessData result = await StartProcess("wsl.exe", serverPath, $"--cd \"{serverPath}\"-- bash -c \"{_linuxGitRev}\"", true);
            return result.Output.TrimEnd();
        }

        private async Task SetCommitOnServer(string newCommit)
        {
            string serverPath = App.GetCurrentProfile().Patches.ServerPatch;
            await StartProcess("wsl.exe", serverPath, $"--cd \"{serverPath}\"-- bash -c \"{_linuxGitReset} && {_linuxGitCheckout} {newCommit}\"", true);
        }

        private async Task PullRepository()
        {
            string serverPath = App.GetCurrentProfile().Patches.ServerPatch;
            await StartProcess("wsl.exe", serverPath, $"--cd \"{serverPath}\"-- bash -c \"{_linuxGitReset} && {_linuxGitPull}\"", true);
        }

        private async Task RunServer(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) return;
            Application.Current.Dispatcher.Invoke(() => _mainFormV.SetProgressLabel("Проверка текущего коммита"));
            string commitOnServer = await GetCommitOnServer();
            string commitOnProfile = App.GetCurrentProfile().ServerCommit;

            if (commitOnProfile == "master" || commitOnProfile == "prod")
            {
                Application.Current.Dispatcher.Invoke(() => _mainFormV.SetProgressLabel("Вытягивание актуального репозитория"));
                await PullRepository();
            }

            if (commitOnProfile != commitOnServer)
            {
                Application.Current.Dispatcher.Invoke(() => _mainFormV.SetProgressLabel("Смена комита"));
                await SetCommitOnServer(commitOnProfile);
            }

            Application.Current.Dispatcher.Invoke(() => _mainFormV.SetProgressLabel("Запуск сервера"));
            ResetProcessField(ref _serverProcess);
            ProcessData result = await StartProcess("wsl.exe", folderPath, $"--cd \"{folderPath}\"-- bash -i -c \"{_linuxZigUsing} & {_linuxCommand}\"", false);
            _serverProcess = result.Process;
            ResetProcessField(ref _serverProcess);
        }

        private async Task RunWinExe(ProfileSettingName app, string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath)) return;
            Application.Current.Dispatcher.Invoke(() => _mainFormV.SetProgressLabel($"Запуск {Path.GetFileName(fullPath)}"));
            ProcessData result = await StartProcess(fullPath, Path.GetDirectoryName(fullPath), "", false);

            switch (app)
            {
                case ProfileSettingName.Hoyo:
                    ResetProcessField(ref _hoyoProcess);
                    _hoyoProcess = result.Process;
                    break;
                case ProfileSettingName.Kcpshim:
                    ResetProcessField(ref _kcpshimProcess);
                    _kcpshimProcess = result.Process;
                    break;
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
            Task.Run(StartLaunching);
        }

        private async Task StartLaunching()
        {
            Profile profile = App.GetCurrentProfile();

            if (profile.Name == string.Empty)
            {
                MessageBox.Show("Профиль не выбран. Пожалуйста выберите профиль");
                return;
            }

            CheckVersionResult checkResult = App.YoshunkoCompatibility.IsCommitVersionCorrect(App.GetCurrentProfile());

            if (checkResult != CheckVersionResult.Correct)
            {
                if (App.YoshunkoCompatibility.AskForContinue(checkResult) == false)
                {
                    return;
                }
            }

            KillAll();
            await RunServer(profile.Patches.ServerPatch);
            await RunWinExe(ProfileSettingName.Hoyo, profile.Patches.HoyoPatch);
            await RunWinExe(ProfileSettingName.Kcpshim, profile.Patches.KcpshimPatch);
            await RunWinExe(ProfileSettingName.Client, profile.Patches.ClientPatch);
            Application.Current.Dispatcher.Invoke(() => _mainFormV.SetProgressLabel(string.Empty));
        }

        private void OnClosed()
        {
            KillAll();
            App.SaveSelectedProfile();
        }
    }
}
