using System.Windows;
using ZZZ_PS_Launcher.WindowI;
using ZZZ_PS_Launcher.WindowP;

namespace ZZZ_PS_Launcher
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class CreateProfileWindow : Window, ICreateProfileWindow
    {
        private Patches _patches;

        public event Action<ProfileSettingName> ClickedSelect;
        public event Action ClickedSave;
        public event Action Showing;
        public event Action Hiding;

        public IPatches Patches => _patches;

        public bool IsDisposed { get; private set; }

        public CreateProfileWindow()
        {
            InitializeComponent();
            CreateProfileWindowP presenter = new(this);
            Closed += (s, e) => IsDisposed = true;
            List<CommitData> items = new();
            items.Add(new CommitData("Самый последний BETA", "master"));
            items.Add(new CommitData("Самый последний PROD", "prod"));
            
            foreach (var commit in App.Commits)
            {
                items.Add(commit);
            }

            ComboBox_Commit.ItemsSource = items;
            ComboBox_Commit.SelectedIndex = 0;
        }

        public void ApplyFromTextBoxes()
        {
            Patches patches = new()
            {
                ServerPatch = TextBox_Server.Text,
                ClientPatch = TextBox_Client.Text,
                HoyoPatch = TextBox_Hoyo.Text,
                KcpshimPatch = TextBox_Kcpshim.Text
            };

            string commit = (string)ComboBox_Commit.SelectedValue;
            Profile profile = new(TextBox_Name.Text, patches, commit);
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

        public string GetTextBox(ProfileSettingName name)
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

        private void ServerButton_Click(object sender, EventArgs e)
        {
            ClickedSelect?.Invoke(ProfileSettingName.Server);
        }

        private void HoyoButton_Click(object sender, EventArgs e)
        {
            ClickedSelect?.Invoke(ProfileSettingName.Hoyo);
        }

        private void KcpshimButton_Click(object sender, EventArgs e)
        {
            ClickedSelect?.Invoke(ProfileSettingName.Kcpshim);
        }

        private void ClientButton_Click(object sender, EventArgs e)
        {
            ClickedSelect?.Invoke(ProfileSettingName.Client);
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            ClickedSave?.Invoke();
        }

        private void SettingsForm_FormClosing(object sender, EventArgs e)
        {
            Hiding?.Invoke();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Showing?.Invoke();
        }
    }
}
