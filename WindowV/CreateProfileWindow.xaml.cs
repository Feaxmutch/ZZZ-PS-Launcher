using System.Windows;
using System.Windows.Controls;

namespace ZZZ_PS_Launcher
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class CreateProfileWindow : Window, ICreateProfileWindow
    {
        private Patches _patches;

        public event Action<ProfileSettingName> ClickedSelect;
        public event Action<ProfileSettingName> ClickedFromProfile;
        public event Action ClickedSave;

        public ServerType[] ServerTypes => Enum.GetValues<ServerType>();
        public ServerType SelectedType { get; set; }

        public IPatches Patches => _patches;

        public bool IsDisposed { get; private set; }

        public CreateProfileWindow()
        {
            InitializeComponent();
            CreateProfileWindowP presenter = new(this);
            Closed += (s, e) => IsDisposed = true;
            List<CommitData> items = new();
            ComboBox_Commit.ItemsSource = new List<CommitData>(App.YoshunkoCommits);
            ComboBox_Commit.SelectedIndex = 0;
            DataContext = this;
        }

        public void ApplyFromView()
        {
            Patches patches = new()
            {
                ServerPatch = TextBox_Server.Text,
                ClientPatch = TextBox_Client.Text,
                HoyoPatch = TextBox_Hoyo.Text,
                KcpshimPatch = TextBox_Kcpshim.Text
            };

            string commit = (string)ComboBox_Commit.SelectedValue;
            Profile profile = new(TextBox_Name.Text, patches, commit, SelectedType);
            App.SaveProfile(profile);
        }

        public void SetTextBox(ProfileSettingName name, string value)
        {
            switch (name)
            {
                case ProfileSettingName.Server:
                    TextBox_Server.Text = value;
                    break;
                case ProfileSettingName.Hoyo:
                    TextBox_Hoyo.Text = value;
                    break;
                case ProfileSettingName.Kcpshim:
                    TextBox_Kcpshim.Text = value;
                    break;
                case ProfileSettingName.Client:
                    TextBox_Client.Text = value;
                    break;
            }
        }

        public void SetTextBox(ProfileSettingName name, Patches patches)
        {
            switch (name)
            {
                case ProfileSettingName.Server:
                    TextBox_Server.Text = patches.ServerPatch;
                    break;
                case ProfileSettingName.Hoyo:
                    TextBox_Hoyo.Text = patches.HoyoPatch;
                    break;
                case ProfileSettingName.Kcpshim:
                    TextBox_Kcpshim.Text = patches.KcpshimPatch;
                    break;
                case ProfileSettingName.Client:
                    TextBox_Client.Text = patches.ClientPatch;
                    break;
            }
        }

        public string GetSetting(ProfileSettingName name)
        {
            switch (name)
            {
                case ProfileSettingName.Server:
                    return TextBox_Server.Text;
                case ProfileSettingName.Hoyo:
                    return TextBox_Hoyo.Text;
                case ProfileSettingName.Kcpshim:
                    return TextBox_Kcpshim.Text;
                case ProfileSettingName.Client:
                    return TextBox_Client.Text;
                case ProfileSettingName.Name:
                    return TextBox_Name.Text;
                case ProfileSettingName.ServerCommit:
                    return (string)ComboBox_Commit.SelectedValue;
            }

            return string.Empty;
        }

        public ServerType GetServerType()
        {
            return SelectedType;
        }

        private void Select_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.Name == Button_ServerSelect.Name)
            {
                ClickedSelect?.Invoke(ProfileSettingName.Server);
            }
            else if (button.Name == Button_HoyoSelect.Name)
            {
                ClickedSelect?.Invoke(ProfileSettingName.Hoyo);
            }
            else if (button.Name == Button_KcpshimSelect.Name)
            {
                ClickedSelect?.Invoke(ProfileSettingName.Kcpshim);
            }
            else if (button.Name == Button_ClientSelect.Name)
            {
                ClickedSelect?.Invoke(ProfileSettingName.Client);
            }
        }

        private void FromProfile_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.Name == Button_ServerProfile.Name)
            {
                ClickedFromProfile?.Invoke(ProfileSettingName.Server);
            }
            else if (button.Name == Button_HoyoProfile.Name)
            {
                ClickedFromProfile?.Invoke(ProfileSettingName.Hoyo);
            }
            else if (button.Name == Button_KcpshimProfile.Name)
            {
                ClickedFromProfile?.Invoke(ProfileSettingName.Kcpshim);
            }
            else if (button.Name == Button_ClientProfile.Name)
            {
                ClickedFromProfile?.Invoke(ProfileSettingName.Client);
            }
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            ClickedSave?.Invoke();
        }

        private void OnServerTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SelectedType)
            {
                case ServerType.Yoshunko:
                    ComboBox_Commit.ItemsSource = new List<CommitData>(App.YoshunkoCommits);
                    break;
                case ServerType.Remielle:
                    ComboBox_Commit.ItemsSource = new List<CommitData>(App.RemielleCommits);
                    break;
            }

            ComboBox_Commit.SelectedIndex = 0;
        }
    }
}
