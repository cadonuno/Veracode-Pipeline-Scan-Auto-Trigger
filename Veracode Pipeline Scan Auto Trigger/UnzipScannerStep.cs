using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veracode_Pipeline_Scan_Auto_Trigger.ScannerSteps;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class UnzipScannerStep : ScannerStep
    {
        public UnzipScannerStep(ProjectOptions projectOptions, GlobalOptions globalOptions, PipelineScanRunner pipelineScanRunner)
            : base("Unzipping pipeline scanner", 3200, 50, "Unable to unzip pipeline scanner", "tar -xf pipeline_scan.zip",
                  new RunScannerStep(projectOptions, globalOptions, pipelineScanRunner), pipelineScanRunner)
        {

        }
    }
}
