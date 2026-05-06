using Microsoft.Win32;

namespace ZZZ_PS_Launcher
{
    public partial class SettingsForm : Form, ISettingsForm
    {
        private Patches _patches;

        public SettingsForm()
        {
            InitializeComponent();
        }

        public event Action<App> ClickedSelect;
        public event Action ClickedSave;
        public event Action Showing;
        public event Action Hiding;

        public IPatches Patches => _patches;

        public void UpdateTextBoxes()
        {
            server_textBox.Text = Program.Patches.ServerPatch;
            kcpshim_textBox.Text = Program.Patches.KcpshimPatch;
            hoyo_textBox.Text = Program.Patches.HoyoPatch;
            client_textBox.Text = Program.Patches.ClientPatch;
        }

        public void ApplyFromTextBoxes()
        {
            Program.SetPatch(App.Server, server_textBox.Text);
            Program.SetPatch(App.Kcpshim, kcpshim_textBox.Text);
            Program.SetPatch(App.Hoyo, hoyo_textBox.Text);
            Program.SetPatch(App.Client, client_textBox.Text);
            Program.SaveSettings();
        }

        private void server_button_Click(object sender, EventArgs e)
        {
            ClickedSelect?.Invoke(App.Server);
        }

        private void hoyo_button_Click(object sender, EventArgs e)
        {
            ClickedSelect?.Invoke(App.Hoyo);
        }

        private void kcpshim_button_Click(object sender, EventArgs e)
        {
            ClickedSelect?.Invoke(App.Kcpshim);
        }

        private void client_button_Click(object sender, EventArgs e)
        {
            ClickedSelect?.Invoke(App.Client);
        }

        private void saveSettings_button_Click(object sender, EventArgs e)
        {
            ClickedSave?.Invoke();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hiding?.Invoke();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Showing?.Invoke();
        }
    }
}
