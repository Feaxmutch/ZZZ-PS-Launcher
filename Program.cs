
using Microsoft.Win32;

namespace ZZZ_PS_Launcher
{
    internal static class Program
    {
        private static SettingsForm _settingsForm;
        private static SettingsFormP _settingsFormP;
        private static Patches _patches;

        public static IPatches Patches => _patches;

        public static string SettingsPath => @"HKEY_CURRENT_USER\Software\ZZZ_PS_Starter";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
            ApplicationConfiguration.Initialize();
            MainForm mainForm = new();
            MainFormP mainFormP = new(mainForm);
            LoadSettings();
            Application.Run(mainForm);
        }

        public static void SaveSettings()
        {
            Registry.SetValue(SettingsPath, PathNames.ServerPath, Patches.ServerPatch);
            Registry.SetValue(SettingsPath, PathNames.KcpshimPath, Patches.KcpshimPatch);
            Registry.SetValue(SettingsPath, PathNames.HoyoSdkPath, Patches.HoyoPatch);
            Registry.SetValue(SettingsPath, PathNames.ClientPath, Patches.ClientPatch);
        }

        public static void LoadSettings()
        {
            _patches.ServerPatch = (string)Registry.GetValue(SettingsPath, PathNames.ServerPath, string.Empty);
            _patches.KcpshimPatch = (string)Registry.GetValue(SettingsPath, PathNames.KcpshimPath, string.Empty);
            _patches.HoyoPatch = (string)Registry.GetValue(SettingsPath, PathNames.HoyoSdkPath, string.Empty);
            _patches.ClientPatch = (string)Registry.GetValue(SettingsPath, PathNames.ClientPath, string.Empty);
        }

        public static void SetPatch(App app, string value)
        {
            switch (app)
            {
                case App.Server:
                    _patches.ServerPatch = value;
                    break;
                case App.Hoyo:
                    _patches.HoyoPatch = value;
                    break;
                case App.Kcpshim:
                    _patches.KcpshimPatch = value;
                    break;
                case App.Client:
                    _patches.ClientPatch = value;
                    break;
            }
        }

        public static void OpenSettings()
        {
            if (_settingsForm == null || _settingsForm.IsDisposed)
            {
                _settingsForm = new();
                _settingsFormP = new(_settingsForm);
            }

            _settingsForm.Show();
        }
    }

    public static class PathNames
    {
        public static string ServerPath => nameof(ServerPath);
        public static string HoyoSdkPath => nameof(HoyoSdkPath);
        public static string KcpshimPath => nameof(KcpshimPath);
        public static string ClientPath => nameof(ClientPath);
    }

    public enum App
    {
        Server,
        Hoyo,
        Kcpshim,
        Client
    }

    public struct Patches : IPatches
    {
        public string ServerPatch { get; set; }
        public string HoyoPatch { get; set; }
        public string KcpshimPatch { get; set; }
        public string ClientPatch { get; set; }
    }

    public interface IPatches
    {
        string ServerPatch { get; }
        string HoyoPatch { get; }
        string KcpshimPatch { get; }
        string ClientPatch { get; }
    }
}