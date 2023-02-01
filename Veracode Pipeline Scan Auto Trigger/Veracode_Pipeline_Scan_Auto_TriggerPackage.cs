using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.VisualStudio.RpcContracts.Solution.ProjectLoadResult;
using Task = System.Threading.Tasks.Task;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(Veracode_Pipeline_Scan_Auto_TriggerPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(GlobalOptions),
    "Pipeline Scan Auto Trigger", "Global Options", 0, 0, true)]
    [ProvideOptionPage(typeof(ProjectOptions),
    "Pipeline Scan Auto Trigger", "Project Options", 0, 0, true)]
    [ProvideToolWindow(typeof(Results_window))]

    public sealed class Veracode_Pipeline_Scan_Auto_TriggerPackage : AsyncPackage
    {
        /// <summary>
        /// Veracode_Pipeline_Scan_Auto_TriggerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "7bb8ced6-257a-4cef-9a8c-46a845903b0e";

        private static Veracode_Pipeline_Scan_Auto_TriggerPackage _instance;
        public static Veracode_Pipeline_Scan_Auto_TriggerPackage Instance 
        { 
            get 
            {
                return _instance;
            } 
        }

        public Veracode_Pipeline_Scan_Auto_TriggerPackage()
        {
            _instance = this;
        }

        public void LoadResultsAndDisplayInResultsWindow(PipelineScanResults pipelineScanResults)
        {
            Results_windowCommand.InitializeToolWindowIfNecessary(this);
            UIThreadHelper.RunOnUIThread(() =>
                    Results_windowCommand.Instance.GetResultsWindow()
                        .IfPresent(resultsWindow => resultsWindow.DisplayResultsAndShowWindow(pipelineScanResults)));
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await StartPipelineScanButton.InitializeAsync(this);
            await Results_windowCommand.InitializeAsync(this);
        }

        #endregion
    }
}
