using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    //TODO: make this project specific
    public class ProjectOptions : DialogPage
    {
        [Category("Project-specific scan configuration")]
        [DisplayName("File to scan")]
        [Description("The file to be scanned. For multiple files, zip them up in the build command")]
        public string FileToScan { get; set; } = "./target/*";

        [Category("Project-specific scan configuration")]
        [DisplayName("Build command")]
        [Description("If present, will be called before the scan to produce a build file")]
        public string BuildCommand { get; set; } = "dotnet publish -c Debug -o <OutputFolder>";

        [Category("Project-specific scan configuration")]
        [DisplayName("Baseline file")]
        [Description("If present, will be used as a baseline file for the scan.")]
        public string BaselineFile { get; set; } = "results.json";

        [Category("When to scan")]
        [DisplayName("Enabled")]
        [Description("Should the plugin be enabled for this project?")]
        public bool IsEnabled { get; set; } = false;

        [Category("When to scan")]
        [DisplayName("Should scan on push")]
        [Description("Enable automatic scan when pushing new code")]
        public bool ShouldScanOnPush { get; set; } = false;
    }
}
