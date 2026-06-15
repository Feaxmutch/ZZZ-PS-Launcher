using System.Diagnostics;

namespace ZZZ_PS_Launcher
{
    public struct ProcessData
    {
        public ProcessData(Process process, string output)
        {
            Process = process;
            Output = output;
        }

        public Process Process { get; }

        public string Output { get; }
    }
}
