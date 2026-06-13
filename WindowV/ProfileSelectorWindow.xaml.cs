using System.Windows;

namespace ZZZ_PS_Launcher
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
            Profile selectedProfile = App.GetCurrentProfile();
            Profile[] profiles = App.GetAllProfiles();
            string[] profileNames = profiles.Select(profile => profile.Name).ToArray();
            ComboBox_Profiles.Items.Clear();

            foreach (var item in profileNames)
            {
                ComboBox_Profiles.Items.Add(item);
            }

            var nameItems = ComboBox_Profiles.Items.Cast<string>().ToList();

            if (selectedProfile.Name != string.Empty)
            {
                ComboBox_Profiles.SelectedIndex = nameItems.IndexOf(nameItems.Where(item => item == selectedProfile.Name).First());
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

        private void OnActivated(object sender, EventArgs e)
        {
            UpdateComboBox();
        }
    }
}
