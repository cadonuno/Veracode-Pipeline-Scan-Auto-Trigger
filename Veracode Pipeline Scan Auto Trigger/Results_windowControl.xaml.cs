using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public partial class Results_windowControl : UserControl
    {
        public ObservableCollection<PipelineScanResultItem> ScanResults { get; set; }

        public Results_window ParentWindow { get; set; }

        public Results_windowControl(Results_window window)
        {
            this.InitializeComponent();
            ParentWindow = window;
            ScanResults = new ObservableCollection<PipelineScanResultItem>();
            resultsGrid.ItemsSource = ScanResults;
        }

        public void ReloadResults(PipelineScanResults scanResults)
        {
            ScanResults.Clear();
            scanResults.GetAsList().ForEach(ScanResults.Add);
        }
    }
}