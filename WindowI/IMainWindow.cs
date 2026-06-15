namespace ZZZ_PS_Launcher
{
    internal interface IMainWindow
    {
        event Action OpeningSettings;
        event Action Launching;
        event Action WindowClosed;

        void SetProgressLabel(string content);
    }
}
