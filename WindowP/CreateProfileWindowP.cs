using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace ZZZ_PS_Launcher
{
    public class CreateProfileWindowP
    {
        private ICreateProfileWindow _windowV;

        public CreateProfileWindowP(ICreateProfileWindow window)
        {
            _windowV = window;
            _windowV.ClickedSelect += OnSelectClick;
            _windowV.ClickedSave += OnSaveClick;
            _windowV.ClickedFromProfile += OnProfileClick;
        }

        private bool FieldsIsValide()
        {
            string serverPath = _windowV.GetSetting(ProfileSettingName.Server);
            string clientPath = _windowV.GetSetting(ProfileSettingName.Client);
            string hoyoPath = _windowV.GetSetting(ProfileSettingName.Hoyo);
            string kcpshimPath = _windowV.GetSetting(ProfileSettingName.Kcpshim);
            string name = _windowV.GetSetting(ProfileSettingName.Name);
            string commit = _windowV.GetSetting(ProfileSettingName.ServerCommit);
            Profile testProfile = new(name, new() { ClientPatch = clientPath, HoyoPatch = hoyoPath, KcpshimPatch = kcpshimPath, ServerPatch = serverPath }, commit, _windowV.GetServerType());

            string messageStart = "Указан неверный путь к";

            if ((Directory.Exists(serverPath + "\\dpsv") && Directory.Exists(serverPath + "\\gamesv")) == false)
            {
                MessageBox.Show($"{messageStart} серверу: {serverPath}");
                return false;
            }

            if (File.Exists(kcpshimPath) == false && testProfile.ServerType == ServerType.Yoshunko)
            {
                MessageBox.Show($"{messageStart} kcpshim: {kcpshimPath}");
                return false;
            }

            if (File.Exists(hoyoPath) == false)
            {
                MessageBox.Show($"{messageStart} hoyo-sdk: {hoyoPath}");
                return false;
            }

            if (File.Exists(clientPath) == false)
            {
                MessageBox.Show($"{messageStart} клиент патчеру: {clientPath}");
                return false;
            }

            if (name == string.Empty)
            {
                MessageBox.Show($"Имя не может быть пустым");
                return false;
            }

            CheckVersionResult checkResult;

            switch (testProfile.ServerType)
            {
                case ServerType.Yoshunko:
                    checkResult = App.YoshunkoCompatibility.IsCommitVersionCorrect(testProfile);

                    if (checkResult != CheckVersionResult.Correct)
                    {
                        if (App.YoshunkoCompatibility.AskForContinue(checkResult) == false)
                        {
                            return false;
                        }
                    }
                    break;
                case ServerType.Remielle:
                    checkResult = App.RemielleCompatibility.IsCommitVersionCorrect(testProfile);

                    if (checkResult != CheckVersionResult.Correct)
                    {
                        if (App.RemielleCompatibility.AskForContinue(checkResult) == false)
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        private void SelectFolder()
        {
            OpenFolderDialog folderDialog = new OpenFolderDialog();

            folderDialog.Title = "Выберите папку с сервером";
            folderDialog.InitialDirectory = @"\\wsl.localhost";

            if (folderDialog.ShowDialog() ?? false)
            {
                _windowV.SetTextBox(ProfileSettingName.Server, folderDialog.FolderName);
            }
        }

        private void SelectExeFile(ProfileSettingName settingName)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Исполняемые файлы (*.exe)|*.exe";
            fileDialog.Title = "Выберите exe файл";

            if (fileDialog.ShowDialog() ?? false)
            {
                _windowV.SetTextBox(settingName, fileDialog.FileName);
            }
        }

        private void OnSelectClick(ProfileSettingName app)
        {
            if (app == ProfileSettingName.Server)
            {
                SelectFolder();
            }
            else
            {
                SelectExeFile(app);
            }
        }

        private void OnProfileClick(ProfileSettingName app)
        {
            ProfileDialogWindow dialog = new();
            dialog.Owner = (_windowV as CreateProfileWindow);
            Profile[] profiles = App.GetAllProfiles();

            if (dialog.ShowDialog() == true)
            {
                Profile selectedProfile = profiles.Where(prof => prof.Name == dialog.SelectedResult).First();
                _windowV.SetTextBox(app, selectedProfile.Patches);
            }
        }

        private void OnSaveClick()
        {
            if (FieldsIsValide())
            {
                if (_windowV.GetServerType() == ServerType.Remielle && _windowV.GetSetting(ProfileSettingName.Kcpshim).Trim() != string.Empty)
                {
                    MessageBox.Show("Вы указали путь к kcpshim, но выбрали тип сервера Remielle. \nkcpshim не требуется сервером и его запуск будет пропущен");
                }

                _windowV.ApplyFromView();
                _windowV.Close();
            }
        }
    }
}
