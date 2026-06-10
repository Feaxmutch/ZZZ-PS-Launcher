namespace ZZZ_PS_Launcher.WindowI
{
    internal interface IMainWindow
    {
        event Action OpeningSettings;
        event Action Launching;
        event Action WindowClosed;
    }
}
