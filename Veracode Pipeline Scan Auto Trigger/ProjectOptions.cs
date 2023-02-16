using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    //TODO: make this project specific
    public class ProjectOptions : DialogPage
    {
        [Category("Project-specific scan configuration")]
        [DisplayName("File to scan")]
        [Description("The file to be scanned. For multiple files, zip them up in the build command")]
        public string FileToScan { get; set; } = "<Project Name>.zip";

        [Category("Project-specific scan configuration")]
        [DisplayName("Build command")]
        [Description("If present, will be called before the scan to produce a build file")]
        public string BuildCommand { get; set; } = "dotnet publish -c Debug -p:PublishProfile=FolderProfile -p:UseAppHost=false -p:SatelliteResourceLanguages='en' -o <Project Name> && rmdir /s /q <Directories to Exclude> && tar -a -c -f <Project Name>.zip <Project Name>";

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
