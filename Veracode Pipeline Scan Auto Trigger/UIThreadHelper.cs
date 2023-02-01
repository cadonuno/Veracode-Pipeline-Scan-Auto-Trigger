using Microsoft.VisualStudio.Shell;
using System;
using System.Windows;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public static class UIThreadHelper
    {
        public static void RunOnUIThread(Action anAction)
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                anAction.Invoke();
            });
        }
    }
}
