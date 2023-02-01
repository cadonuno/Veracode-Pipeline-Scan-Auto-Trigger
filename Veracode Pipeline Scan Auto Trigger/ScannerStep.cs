using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger.ScannerSteps
{
    public abstract class ScannerStep

    {
        protected string _runningMessage;
        protected int _maxValueForStep;
        protected int _incrementValueForStep;
        protected string _errorMessageIfFail;
        protected string _commandToRun;
        protected ScannerStep _nextStep;
        protected PipelineScanRunner _pipelineScanRunner;

        protected ScannerStep(string runningMessage, int maxValueForStep, int incrementValueForStep, string errorMessageIfFail, string commandToRun, ScannerStep nextStep, PipelineScanRunner pipelineScanRunner)
        {
            _runningMessage = runningMessage;
            _maxValueForStep = maxValueForStep;
            _incrementValueForStep = incrementValueForStep;
            _errorMessageIfFail = errorMessageIfFail;
            _nextStep = nextStep;
            _pipelineScanRunner = pipelineScanRunner;
            _commandToRun = commandToRun;
        }

        public virtual void Run()
        {
            _pipelineScanRunner.TryRunNewStage(_runningMessage, _maxValueForStep, _incrementValueForStep,
                _commandToRun, SuccessAction, FailAction);
        }

        protected virtual void SuccessAction()
        {
            _pipelineScanRunner.FinishAction(_maxValueForStep);
            _nextStep?.Run();
        }

        protected virtual void FailAction(int returnCode)
        {
            _pipelineScanRunner.FinishAction(_maxValueForStep);
            _pipelineScanRunner.TryFailAndShowMessageAndOsCommandError(_errorMessageIfFail);
        }
    }
}
