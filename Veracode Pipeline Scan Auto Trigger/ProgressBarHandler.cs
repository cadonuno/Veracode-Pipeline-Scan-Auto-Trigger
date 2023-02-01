using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class ProgressBarHandler
    {
        private static ProgressBarHandler _instance;


        private ProgressBarForm _progressBarForm;

        private ProgressBarHandler(Action onClose)
        {
            _progressBarForm = new ProgressBarForm(onClose);
            _progressBarForm.Show();
        }

        public static ProgressBarHandler Initialize(string message, Action onClose)
        {
            _instance = new ProgressBarHandler(onClose);
            _instance.SetMessage(message);
            return _instance;
        }

        public void SetMessage(string message)
        {
            _progressBarForm.SetMessage(message);
        }

        public void CleanUpAndClose()
        {
            _instance = null;
            _progressBarForm.CloseForm();
        }

        public void SetMaxValueForCurrentSection(int maxValue)
        {
            _progressBarForm.SetMaxValueForCurrentSection(maxValue);
        }

        public void SetValueIncrement(int valueIncrement)
        {
            _progressBarForm.ValueIncrement = valueIncrement;
        }

        public void HandleOutputStream(string dataToWrite)
        {
            if (dataToWrite != null)
            {
                _progressBarForm.AddDetails(dataToWrite);
            }
        }

        public void ClearDetails()
        {
            _progressBarForm.ClearDetails();
        }

        public void SetCurrentValue(int valueToSet)
        {
            _progressBarForm.SetCurrentValue(valueToSet);
        }

        public void FinalizeExecution()
        {
            _progressBarForm.FinalizeExecution();
        }
    }
}
