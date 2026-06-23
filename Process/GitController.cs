namespace ZZZ_PS_Launcher
{
    public class GitController
    {
        private ProcessStarter _processStarter = new();
        private string _git = "git";
        private string _pull = "pull";
        private string _checkout = "checkout";
        private string _reset = "reset --hard HEAD";
        private string _symbolicRef = "symbolic-ref --short -q HEAD";
        private string _revParse = "rev-parse --short HEAD";

        public Exicutor Exicutor { get; set; }

        public async Task Pull(string repPath)
        {
            switch (Exicutor)
            {
                case Exicutor.Wsl:
                    await _processStarter.StartProcess("wsl.exe", repPath, $"-- bash -c \"{_git} {_reset} && {_git} {_pull}\"", true);
                    break;
                case Exicutor.Git:
                    await _processStarter.StartProcess("git", repPath, $"{_reset}", true);
                    await _processStarter.StartProcess("git", repPath, $"{_pull}", true);
                    break;
            }
        }

        public async Task Checkout(string repPath, string newCommit)
        {
            switch (Exicutor)
            {
                case Exicutor.Wsl:
                    await _processStarter.StartProcess("wsl", repPath, $"-- bash -c \"{_git} {_reset} && {_git} {_checkout} {newCommit}\"", true);
                    break;
                case Exicutor.Git:
                    await _processStarter.StartProcess("git", repPath, $"{_reset}", true);
                    await _processStarter.StartProcess("git", repPath, $"{_checkout} {newCommit}", true);
                    break;
            }
        }

        public async Task<string> GetCurrentCommit(string repPath)
        {
            switch (Exicutor)
            {
                case Exicutor.Wsl:
                    return _processStarter.StartProcess("wsl", repPath, $"-- bash -c \"{_git} {_revParse}\"", true).Result.Output;
                case Exicutor.Git:
                    await _processStarter.StartProcess("git", repPath, $"{_symbolicRef}", false);
                    return _processStarter.StartProcess("git", repPath, $"{_revParse}", true).Result.Output.Trim();
            }

            return string.Empty;
        }
    }

    public enum Exicutor
    {
        Wsl,
        Git
    }
}
