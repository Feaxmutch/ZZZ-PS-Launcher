using System.Diagnostics;

namespace ZZZ_PS_Launcher
{
    public struct ProcessData
    {
        public ProcessData(Process process, string output, string error = "")
        {
            Process = process;
            Output = output;
        }

        public Process Process { get; }

        public string Output { get; }

        public string Error { get; }
    }
}
