using Microsoft.Win32;

namespace ZZZ_PS_Launcher
{
    internal static class Program
    {
        public static string SettingsPath => @"HKEY_CURRENT_USER\Software\ZZZ_PS_Starter";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }



    public static class PathNames
    {
        public static string ServerPath => nameof(ServerPath);
        public static string HoyoSdkPath => nameof(HoyoSdkPath);
        public static string KcpshimPath => nameof(KcpshimPath);
        public static string ClientPath => nameof(ClientPath);
    }
}