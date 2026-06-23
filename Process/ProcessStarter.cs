using System.Diagnostics;
using System.Windows;

namespace ZZZ_PS_Launcher
{
    public class ProcessStarter
    {
        public async Task<ProcessData> StartProcess(string fileName, string directory, string arguments, bool canOutput = false, bool asAdmin = true)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = fileName,
                WorkingDirectory = directory,
                Arguments = arguments,
                UseShellExecute = !canOutput,
                RedirectStandardOutput = canOutput,
                RedirectStandardError = canOutput,
                WindowStyle = ProcessWindowStyle.Minimized,
                Verb = asAdmin ? "runas" : "",
            };
            Process process = null;

            try
            {
                process = Process.Start(psi);

                if (process != null && canOutput)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    return new ProcessData(process, output);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска процесса: {ex.Message}");
            }

            return new ProcessData(process, string.Empty);
        }
    }
}
