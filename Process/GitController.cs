using System;
using System.Collections.Generic;
using System.Text;

namespace ZZZ_PS_Launcher
{
    public class GitController
    {
        private ProcessStarter _processStarter = new();
        private string _pull = "git pull";
        private string _checkout = "git checkout";
        private string _reset = "git reset --hard HEAD";
        private string _revParse = "git symbolic-ref --short -q HEAD || git rev-parse --short HEAD";

        private Exicutor Exicutor { get; set; }

        public async Task Pull(string repPath)
        {
            switch (Exicutor)
            {
                case Exicutor.Wsl:
                    await _processStarter.StartProcess("wsl.exe", repPath, $"--cd \"{repPath}\"-- bash -c \"{_reset} && {_pull}\"", true);
                    break;
                case Exicutor.PowerShell:
                    break;
            }
        }

        public async Task Checkout(string repPath, string newCommit)
        {
            switch (Exicutor)
            {
                case Exicutor.Wsl:
                    await _processStarter.StartProcess("wsl.exe", repPath, $"--cd \"{repPath}\"-- bash -c \"{_reset} && {_checkout} {newCommit}\"", true);
                    break;
                case Exicutor.PowerShell:
                    break;
            }
        }

        public async Task<string> GetCurrentCommit(string repPath)
        {
            switch (Exicutor)
            {
                case Exicutor.Wsl:
                    return _processStarter.StartProcess("wsl.exe", repPath, $"--cd \"{repPath}\"-- bash -c \"{_revParse}\"", true).Result.Output;
                case Exicutor.PowerShell:
                    return string.Empty;
            }

            return string.Empty;
        }
    }

    public enum Exicutor
    {
        Wsl,
        PowerShell
    }
}
