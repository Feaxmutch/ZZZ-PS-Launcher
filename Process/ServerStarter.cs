namespace ZZZ_PS_Launcher
{
    public abstract class ServerStarter
    {
        protected ProcessStarter ProcessStarter { get; private set; } = new();

        protected abstract string ServerCommand { get; }

        protected abstract string Application { get; }

        public async Task<ProcessData> StartServer(string serverPath)
        {
            return await StartProsess(serverPath);
        }

        protected abstract Task<ProcessData> StartProsess(string serverPath);


    }
}
