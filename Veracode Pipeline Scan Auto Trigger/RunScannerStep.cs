using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Veracode_Pipeline_Scan_Auto_Trigger.ScannerSteps;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class RunScannerStep : ScannerStep
    {
        private const string SCAN_OUTPUT_FILE_NAME = "pipeline_results.json";
        private GlobalOptions _globalOptions;
        private ProjectOptions _projectOptions;

        public RunScannerStep(ProjectOptions projectOptions, GlobalOptions globalOptions, PipelineScanRunner pipelineScanRunner)
            : base("Running pipeline scanner", 10000, 20, "Unable to run the Veracode pipeline scanner", GetCommandToRun(globalOptions, projectOptions),
                  null, pipelineScanRunner)
        {
            _projectOptions = projectOptions;
            _globalOptions = globalOptions;
        }

        public override void Run()
        {
            if (File.Exists(SCAN_OUTPUT_FILE_NAME))
            {
                File.Delete(SCAN_OUTPUT_FILE_NAME);
            }
            base.Run();
        }

        override protected void SuccessAction()
        {
            base.SuccessAction();
            _pipelineScanRunner.SetAsSuccess("No" + (IsNullOrEmpty(_projectOptions.BaselineFile) ? "" : " new") + " vunerabilities found during pipeline scan!");

        }

        override protected void FailAction(int returnCode)
        {
            if (File.Exists(SCAN_OUTPUT_FILE_NAME))
            {
                _pipelineScanRunner.FinishAction(_maxValueForStep);
                _ = Task.Run(() => _pipelineScanRunner.SetAsFailure(returnCode + (IsNullOrEmpty(_projectOptions.BaselineFile) ? "" : " new") + " vunerabilities found during pipeline scan!"));
                PipelineScanResults.Of(SCAN_OUTPUT_FILE_NAME)
                    .IfPresent(pipelineScanResults => 
                        Veracode_Pipeline_Scan_Auto_TriggerPackage.Instance.LoadResultsAndDisplayInResultsWindow(pipelineScanResults));                
            }
            else
            {
                base.FailAction(returnCode);
            }
        }

        private static string GetCommandToRun(GlobalOptions globalOptions, ProjectOptions projectOptions)
        {            
            StringBuilder commandBuilder = new StringBuilder("java -jar pipeline-scan.jar");
            TryAddParameter(commandBuilder, "fail_on_severity",
                globalOptions, projectOptions,
                criteriaToAdd: null,
                addingFunction: () => AppendFailOnSeverityParameters(globalOptions),
                isQuoted: true);
            TryAddParameter(commandBuilder, "file",
                globalOptions, projectOptions,
                criteriaToAdd: null,
                addingFunction: () => projectOptions.FileToScan,
                isQuoted: true);
            TryAddParameter(commandBuilder, "json_output_file",
                globalOptions, projectOptions,
                criteriaToAdd: null,
                addingFunction: () => SCAN_OUTPUT_FILE_NAME,
                isQuoted: true);

            APICredentials apiCredentials = APICredentials.FromSettings(globalOptions);
            TryAddParameter(commandBuilder, "veracode_api_id",
                globalOptions, projectOptions,
                criteriaToAdd: null,
                addingFunction: () => apiCredentials.VeracodeApiId,
                isQuoted: true);
            TryAddParameter(commandBuilder, "veracode_api_key",
                globalOptions, projectOptions,
                criteriaToAdd: null,
                addingFunction: () => apiCredentials.VeracodeApiKey,
                isQuoted: true);

            TryAddParameter(commandBuilder, "baseline_file",
                globalOptions, projectOptions,
                criteriaToAdd: () => !IsNullOrEmpty(projectOptions.BaselineFile),
                addingFunction: () => projectOptions.BaselineFile,
                isQuoted: true);
            return commandBuilder.ToString();
        }

        private static void TryAddParameter(
            StringBuilder commandBuilder, string fieldToAdd, 
            GlobalOptions globalOptions, ProjectOptions projectOptions,
            Func<bool> criteriaToAdd, Func<string> addingFunction, bool isQuoted)
        {
            if (criteriaToAdd == null || criteriaToAdd.Invoke())
            {
                string parameterValue = addingFunction.Invoke();
                if (!IsNullOrEmpty(parameterValue))
                {
                    commandBuilder
                        .Append(" --")
                        .Append(fieldToAdd)
                        .Append(" ")
                        .Append(isQuoted ? "\"" : "")
                        .Append(parameterValue)
                        .Append(isQuoted ? "\"" : "");
                }
            }
        }

        private static string AppendFailOnSeverityParameters(GlobalOptions globalOptions)
        {
            StringBuilder commandBuilder = new StringBuilder();
            bool hasAdded = false;
            if (globalOptions.ShouldFailOnVeryHigh)
            {
                commandBuilder.Append("VeryHigh");
                hasAdded = true;
            }
            if (globalOptions.ShouldFailOnHigh)
            {
                if (hasAdded)
                {
                    commandBuilder.Append(", ");
                }
                commandBuilder.Append("High");
                hasAdded = true;
            }
            if (globalOptions.ShouldFailOnMedium)
            {
                if (hasAdded)
                {
                    commandBuilder.Append(", ");
                }
                commandBuilder.Append("Medium");
                hasAdded = true;
            }
            if (globalOptions.ShouldFailOnLow)
            {
                if (hasAdded)
                {
                    commandBuilder.Append(", ");
                }
                commandBuilder.Append("Low");
                hasAdded = true;
            }
            if (globalOptions.ShouldFailOnInformational)
            {
                if (hasAdded)
                {
                    commandBuilder.Append(", ");
                }
                commandBuilder.Append("Informational");
            }
            return commandBuilder.ToString();
        }

        private static bool IsNullOrEmpty(string aString)
        {
            return aString == null || aString.Trim() == "";
        }
    }
}
