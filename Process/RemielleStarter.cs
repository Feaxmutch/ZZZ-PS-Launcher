namespace ZZZ_PS_Launcher
{
    public class RemielleStarter : ServerStarter
    {
        protected override string ServerCommand => "zig build serve-all";

        protected override string Application => "powershell.exe";

        protected override async Task<ProcessData> StartProsess(string serverPath)
        {
            return await ProcessStarter.StartProcess(Application, serverPath, $"-NoExit -Command \"{ServerCommand}\"", false, false);
        }
    }
}
