using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class OsCommandRunner
    {
        private Process _process;
        private ProgressBarHandler _progressBarHandler;

        public void RunCommand(string commandToRun, ProgressBarHandler progressBarHandler, Action buildPassed, Action<int> buildFailed)
        {
            _process = new Process();
            _progressBarHandler = progressBarHandler;
            _progressBarHandler.ClearDetails();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + commandToRun,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            _process.StartInfo = startInfo;
            _process.EnableRaisingEvents = true;
            _process.OutputDataReceived += p_OutputDataReceived;
            _process.Start();
            _process.BeginOutputReadLine();

            _process.Exited += delegate {
                if (!_process.HasExited || _process.ExitCode != 0)
                {
                    buildFailed.Invoke(_process.ExitCode);
                } 
                else
                {
                    buildPassed.Invoke();
                }
            };
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs eventArguments)
        {
            _progressBarHandler.HandleOutputStream(eventArguments.Data);
        }

        public string GetStdErr()
        {
            return _process.StandardError.ReadToEnd();
        }
    }

}