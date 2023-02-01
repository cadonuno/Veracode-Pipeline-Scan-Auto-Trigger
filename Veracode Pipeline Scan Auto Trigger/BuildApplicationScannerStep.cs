using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger.ScannerSteps
{
    public class BuildApplicationScannerStep : ScannerStep
    {
        public BuildApplicationScannerStep(ProjectOptions projectOptions, GlobalOptions globalOptions, PipelineScanRunner pipelineScanRunner) 
            : base ("Building application", 2500, 100, "Unable to build application", projectOptions.BuildCommand,
                  new DownloadScannerStep(projectOptions, globalOptions, pipelineScanRunner), pipelineScanRunner)
        {

        }
    }
}
