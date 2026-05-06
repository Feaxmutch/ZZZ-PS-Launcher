
namespace ZZZ_PS_Launcher
{
    internal class SettingsFormP
    {
        private ISettingsForm _settingsFormV;

        public SettingsFormP(ISettingsForm settingsForm)
        {
            _settingsFormV = settingsForm;
            _settingsFormV.ClickedSelect += OnSelectClick;
            _settingsFormV.ClickedSave += OnSaveClick;
            _settingsFormV.Showing += OnShowing;
            _settingsFormV.Hiding += OnHiding;
        }

        private void SelectFolder()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку с сервером";
                folderDialog.UseDescriptionForTitle = true;
                folderDialog.InitialDirectory = @"\\wsl.localhost";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    Program.SetPatch(App.Server, folderDialog.SelectedPath);
                    _settingsFormV.UpdateTextBoxes();
                }
            }
        }

        private void SelectExeFile(App app)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Исполняемые файлы (*.exe)|*.exe";
                fileDialog.Title = "Выберите exe файл";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Program.SetPatch(app, fileDialog.FileName);
                    _settingsFormV.UpdateTextBoxes();
                }
            }
        }

        private void OnSelectClick(App app)
        {
            if (app == App.Server)
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
            _settingsFormV.ApplyFromTextBoxes();
            _settingsFormV.Close();
        }

        private void OnShowing()
        {
            _settingsFormV.UpdateTextBoxes();
        }

        private void OnHiding()
        {
            _settingsFormV.ApplyFromTextBoxes();
        }
    }
}
