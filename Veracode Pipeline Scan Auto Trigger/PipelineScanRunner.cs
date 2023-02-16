using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.CodeDom;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Windows.Forms;
using Veracode_Pipeline_Scan_Auto_Trigger.ScannerSteps;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class PipelineScanRunner 
    {
        private static bool _hasStarted;

        private bool _hasStopped = false;
        private bool _scanPassed = false;
        private bool _hasFinished = false;
        private bool _didStartThisOne = false;
        private bool _hasThrownError;
        private ProgressBarHandler _progressBarHandler;
        private OsCommandRunner _osCommandRunner;
        private AsyncPackage _asyncPackage;
        private ProjectOptions _projectOptions;

        public bool ScanPassed()
        {
            return _scanPassed;
        }

        public bool HasFinished()
        {
            return _hasFinished;
        }

        public bool DidStartThisOne()
        {
            return _didStartThisOne;
        }

        public bool HasStopped()
        {
            return _hasStopped;
        }

        public void RunScanIfNecessary(ProjectOptions projectOptions, GlobalOptions globalOptions, AsyncPackage asyncPackage)
        {
            if (_hasStarted || !projectOptions.IsEnabled)
            {
                return;
            }
            _didStartThisOne = true;
            _hasStarted = true;
            _hasStopped = false;
            _asyncPackage = asyncPackage;
            _projectOptions = projectOptions;
            if (!_projectOptions.IsEnabled)
            {
                _scanPassed = true;
                _hasFinished = true;
                return;
            }
            _osCommandRunner = new OsCommandRunner();
            _progressBarHandler = ProgressBarHandler.Initialize("Initializing Scan", OnCloseProgressBar);
            if (_projectOptions.BuildCommand != null)
            {
                new BuildApplicationScannerStep(_projectOptions, globalOptions, this).Run();
            }
            else
            {
                new DownloadScannerStep(_projectOptions, globalOptions, this).Run();
            }
        }

        private void OnCloseProgressBar()
        {
            if (_hasThrownError)
            {
                _progressBarHandler.CleanUpAndClose();
                FinishProcess(false);
            }
            else
            {
                _hasStopped = true;
                _hasStarted = false;
            }
        }

        private void FinishProcess(bool hasPassed)
        {
            _scanPassed = hasPassed;
            _hasFinished = true;
            _hasStarted = false;
        }

        public void TryRunNewStage(string baseMessage, int progressBarMaxValue, 
            int progressBarIncrementValue, string commandToRun, Action successAction, Action<int> failureAction)
        {
            if (!_hasStopped)
            {
                _progressBarHandler.SetMessage(baseMessage);
                _progressBarHandler.SetMaxValueForCurrentSection(progressBarMaxValue);
                _progressBarHandler.SetValueIncrement(progressBarIncrementValue);

                _osCommandRunner.RunCommand(commandToRun, _progressBarHandler,
                    successAction, failureAction);
            }
        }

        public void TryFailAndShowMessageAndOsCommandError(string errorMessage)
        {
            if (!_hasStopped)
            {
                MessageUtils.ShowMessage(_asyncPackage, errorMessage, _osCommandRunner.GetStdErr());
                _hasThrownError = true;
                _hasStarted = false;
                _progressBarHandler.FinalizeExecution();
            }
        }

        public void SetAsSuccess(string message)
        {
            if (!_hasStopped)
            {
                MessageUtils.ShowMessage(_asyncPackage, message, null);
                _progressBarHandler.CleanUpAndClose();
                _hasFinished = true;
                _scanPassed = true;
                _hasStarted = false;
            }
        }

        public void SetAsFailure(string message)
        {
            if (!_hasStopped)
            {
                MessageUtils.ShowMessage(_asyncPackage, message, null);
                _progressBarHandler.FinalizeExecution();
                _hasFinished = true;
                _scanPassed = false;
            }
        }

        public void FinishAction(int maxValueForStep)
        {
            _progressBarHandler.SetCurrentValue(maxValueForStep);
        }
    }
}
