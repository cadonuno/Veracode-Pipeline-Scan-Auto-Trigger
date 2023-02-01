using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veracode_Pipeline_Scan_Auto_Trigger.ScannerSteps;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class DownloadScannerStep : ScannerStep
    {
        public DownloadScannerStep(ProjectOptions projectOptions, GlobalOptions globalOptions, PipelineScanRunner pipelineScanRunner)
            : base("Downloading pipeline scanner zip", 3000, 100, "Unable to download pipeline scanner",
                  "curl.exe --output pipeline_scan.zip --url https://downloads.veracode.com/securityscan/pipeline-scan-LATEST.zip",
                  new UnzipScannerStep(projectOptions, globalOptions, pipelineScanRunner), pipelineScanRunner)
        {

        }
    }
}
