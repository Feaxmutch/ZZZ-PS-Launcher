namespace ZZZ_PS_Launcher.WindowI
{
    internal interface IMainWindow
    {
        event Action OpeningSettings;
        event Action LaunchingServer;
        event Action LaunchingClient;
    }
}
