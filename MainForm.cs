
namespace ZZZ_PS_Launcher
{
    public partial class MainForm : Form, IMainForm
    {
        public event Action OpeningSettings;
        public event Action LaunchingServer;
        public event Action LaunchingClient;

        public MainForm()
        {
            InitializeComponent();
        }

        private void openSettings_button_Click(object sender, EventArgs e)
        {
            OpeningSettings?.Invoke();
        }

        private void launchServer_button_Click(object sender, EventArgs e)
        {
            LaunchingServer?.Invoke();
        }

        private void launchClient_button_Click(object sender, EventArgs e)
        {
            LaunchingClient?.Invoke();
        }
    }
}
