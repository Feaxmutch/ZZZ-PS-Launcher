namespace ZZZ_PS_Launcher
{
    public class YoshunkoStarter : ServerStarter
    {
        protected override string ServerCommand => "zig build run-dpsv & zig build run-gamesv";

        protected override string Application => "wsl.exe";

        protected override async Task<ProcessData> StartProsess(string serverPath)
        {
            return await ProcessStarter.StartProcess(Application, serverPath, $"-- bash -i -c \"zvm use rr & {ServerCommand}\"", false);
        }
    }
}
