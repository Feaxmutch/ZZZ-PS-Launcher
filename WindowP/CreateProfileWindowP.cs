using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using ZZZ_PS_Launcher;

namespace ZZZ_PS_Launcher
{
    internal class CreateProfileWindowP
    {
        private ICreateProfileWindow _windowV;

        public CreateProfileWindowP(ICreateProfileWindow window)
        {
            _windowV = window;
            _windowV.ClickedSelect += OnSelectClick;
            _windowV.ClickedSave += OnSaveClick;
        }

        private bool FieldsIsValide()
        {
            string serverPath = _windowV.GetTextBox(ProfileSettingName.Server);
            string clientPath = _windowV.GetTextBox(ProfileSettingName.Client);
            string hoyoPath = _windowV.GetTextBox(ProfileSettingName.Hoyo);
            string kcpshimPath = _windowV.GetTextBox(ProfileSettingName.Kcpshim);
            string name = _windowV.GetTextBox(ProfileSettingName.Name);
            string messageStart = "Указан неверный путь к";

            if ((Directory.Exists(serverPath + "\\dpsv") && Directory.Exists(serverPath + "\\gamesv")) == false)
            {
                MessageBox.Show($"{messageStart} серверу: {serverPath}");
                return false;
            }

            if (File.Exists(kcpshimPath) == false)
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
                MessageBox.Show($"{messageStart} yidhari: {clientPath}");
                return false;
            }

            if (name == string.Empty)
            {
                MessageBox.Show($"Имя не может быть пустым");
                return false;
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

        private void OnSaveClick()
        {
            if (FieldsIsValide())
            {
                _windowV.ApplyFromTextBoxes();
                _windowV.Close();
            }
        }
    }
}
