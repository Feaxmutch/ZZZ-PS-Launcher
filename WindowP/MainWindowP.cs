using System.Diagnostics;
using System.IO;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZZZ_PS_Launcher
{
    class MainWindowP
    {
        private static ProfileSelectorWindow _profileSelector;
        private IMainWindow _mainFormV;
        private Process _serverProcess;
        private Process _hoyoProcess;
        private Process _kcpshimProcess;
        private GitController _gitController = new();
        private ProcessStarter _processStarter = new();
        private YoshunkoStarter _yoshunkoStarter = new();
        private RemielleStarter _remielleStarter = new();


        public MainWindowP(IMainWindow window)
        {
            _mainFormV = window;
            _mainFormV.OpeningSettings += OnOpenSettings;
            _mainFormV.Launching += OnLaunchClient;
            _mainFormV.WindowClosed += OnClosed;
        }

        private async Task RunServer(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) return;
            string severPath = App.GetCurrentProfile().Patches.ServerPatch;
            UpdateLabel("Проверка текущего коммита");
            string commitOnServer = await _gitController.GetCurrentCommit(severPath);
            Profile profile = App.GetCurrentProfile();
            string commitOnProfile = App.GetCurrentProfile().ServerCommit;

            if (commitOnProfile == "master" || commitOnProfile == "prod")
            {
                UpdateLabel("Вытягивание актуального репозитория");
                await _gitController.Pull(severPath);
            }

            if (commitOnProfile != commitOnServer)
            {
                UpdateLabel("Смена комита");
                await _gitController.Checkout(severPath, commitOnProfile);
            }

            UpdateLabel("Запуск сервера");
            ResetProcessField(ref _serverProcess);
            ProcessData result = new();

            switch (profile.ServerType)
            {
                case ServerType.Yoshunko:
                    result = await _yoshunkoStarter.StartServer(folderPath);
                    break;
                case ServerType.Remielle:
                    result = await _remielleStarter.StartServer(folderPath);
                    break;
            }
           
            _serverProcess = result.Process;
            ResetProcessField(ref _serverProcess);
        }

        private async Task RunWinExe(ProfileSettingName app, string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath)) return;
            UpdateLabel($"Запуск {Path.GetFileName(fullPath)}");
            ProcessData result = await _processStarter.StartProcess(fullPath, Path.GetDirectoryName(fullPath), "", false);

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

            CheckVersionResult checkResult;

            switch (profile.ServerType)
            {
                case ServerType.Yoshunko:
                    checkResult = App.YoshunkoCompatibility.IsCommitVersionCorrect(profile);

                    if (checkResult != CheckVersionResult.Correct)
                    {
                        if (App.YoshunkoCompatibility.AskForContinue(checkResult) == false)
                        {
                            return;
                        }
                    }
                    break;
                case ServerType.Remielle:
                    checkResult = App.RemielleCompatibility.IsCommitVersionCorrect(profile);

                    if (checkResult != CheckVersionResult.Correct)
                    {
                        if (App.RemielleCompatibility.AskForContinue(checkResult) == false)
                        {
                            return;
                        }
                    }
                    break;
            }

            KillAll();
            await RunServer(profile.Patches.ServerPatch);
            await RunWinExe(ProfileSettingName.Hoyo, profile.Patches.HoyoPatch);
            if (profile.ServerType == ServerType.Yoshunko) await RunWinExe(ProfileSettingName.Kcpshim, profile.Patches.KcpshimPatch);
            await RunWinExe(ProfileSettingName.Client, profile.Patches.ClientPatch);
            UpdateLabel(string.Empty);
        }

        private void OnClosed()
        {
            KillAll();
            App.SaveSelectedProfile();
        }

        private void UpdateLabel(string content)
        {
            Application.Current.Dispatcher.Invoke(() => _mainFormV.SetProgressLabel(content));
        }
    }
}
