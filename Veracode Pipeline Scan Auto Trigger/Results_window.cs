using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("e59af939-7e03-49f7-8592-64021a3aeb03")]
    public class Results_window : ToolWindowPane
    {
        public const string Title = "Pipeline Scan Results";

        /// <summary>
        /// Initializes a new instance of the <see cref="Results_window"/> class.
        /// </summary>
        public Results_window() : base()
        {
            Caption = Title;
            BitmapImageMoniker = KnownMonikers.ImageIcon;
            Content = new Results_windowControl(this);
        }

        public void AddResultsAndShowWindow(PipelineScanResults scanResults)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ((Results_windowControl)this.Content).ReloadResults(scanResults);
            ((IVsWindowFrame)Frame).Show();
        }
    }
}
