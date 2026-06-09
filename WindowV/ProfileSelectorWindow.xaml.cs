using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZZZ_PS_Launcher_2.WindowV
{
    /// <summary>
    /// Логика взаимодействия для ProfileSelector.xaml
    /// </summary>
    public partial class ProfileSelectorWindow : Window
    {
        CreateProfileWindow _window;

        public bool IsDisposed { get; private set; }

        public ProfileSelectorWindow()
        {
            InitializeComponent();
            Closed += (s, e) => IsDisposed = true;
            UpdateComboBox();
        }

        private void UpdateComboBox()
        {
            Profile[] profiles = App.GetAllProfiles();
            string[] profileNames = profiles.Select(profile => profile.Name).ToArray();
            ComboBox_Profiles.Items.Clear();

            foreach (var name in profileNames)
            {
                ComboBox_Profiles.Items.Add(name);
            }

            if (App.GetCurrentProfile().Name != string.Empty)
            {
                ComboBox_Profiles.SelectedIndex = ComboBox_Profiles.Items.IndexOf(App.GetCurrentProfile().Name);
            }
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            if (_window == null || _window.IsDisposed)
            {
                _window = new();
            }

            _window.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ComboBox_Profiles.Items.Count == 0)
            {
                return;
            }

            if (ComboBox_Profiles.SelectedIndex >= 0)
            {
                string selectedProfile = (string)ComboBox_Profiles.Items[ComboBox_Profiles.SelectedIndex];
                Profile[] profiles = App.GetAllProfiles();
                Profile profile = profiles.Where(prof => prof.Name == selectedProfile).First();
                App.SetProfile(profile);
            }
        }
    }
}
