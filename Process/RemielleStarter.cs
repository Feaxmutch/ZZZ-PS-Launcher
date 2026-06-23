namespace ZZZ_PS_Launcher
{
    public class RemielleStarter : ServerStarter
    {
        protected override string ServerCommand => "Start-Process zig -ArgumentList 'build serve-dp' -NoNewWindow; zig build serve-game";

        protected override string Application => "powershell.exe";

        protected override async Task<ProcessData> StartProsess(string serverPath)
        {
            return await ProcessStarter.StartProcess(Application, serverPath, $"-NoExit -Command \"{ServerCommand}\"", false, false);
        }
    }
}
