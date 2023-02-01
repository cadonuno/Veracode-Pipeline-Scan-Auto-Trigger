using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class ProgressBarForm : Form
    {
        private string _baseMessage;
        private int _currentIndex = 0;
        private string[] _elipsisArray = { "", ".", "..", "..."};
        private Timer _timer;
        private bool _isClosing;

        private ProgressBar progressBar;
        private TextBox descriptionTextBox;
        private TextBox detailsTextBox;
        private Button cancelButton;
        private Action _onCancel;
        private int _maxValueForCurrentSection;

        public int ValueIncrement { get; set; }

        public ProgressBarForm(Action onCancel)
        {
            _isClosing = false;
            InitializeComponent();
            _onCancel = onCancel;
        }

        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.cancelButton = new System.Windows.Forms.Button();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(22, 44);
            this.progressBar.Maximum = 10000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(716, 29);
            this.progressBar.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(631, 266);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(107, 34);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(22, 12);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(716, 26);
            this.descriptionTextBox.TabIndex = 2;
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.Location = new System.Drawing.Point(22, 79);
            this.detailsTextBox.Multiline = true;
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.detailsTextBox.Size = new System.Drawing.Size(715, 169);
            this.detailsTextBox.TabIndex = 3;
            // 
            // ProgressBarForm
            // 
            this.AcceptButton = this.cancelButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(767, 311);
            this.Controls.Add(this.detailsTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "ProgressBarForm";
            this.Text = "Veracode Pipeline Scan";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            _isClosing = true;
            _onCancel.Invoke();
            Dispose();
        }

        public void SetMessage(string message)
        {
            _baseMessage = message;
            if (_timer == null)
            {
                _timer = new Timer
                {
                    Interval = 200
                };
                _timer.Tick += DoTimerTick;
                _timer.Enabled = true;
            }
            
        }

        private void DoTimerTick(object sender, EventArgs e)
        {
            if (descriptionTextBox == null)
            {
                return;
            }
            RunOnUIThread(() => SetProgressBarValue(progressBar.Value + ValueIncrement));
            _currentIndex++;
            if (_currentIndex > 3)
            {
                _currentIndex = 0;
            }
            descriptionTextBox.Text = _baseMessage + _elipsisArray[_currentIndex];
            
        }

        private void SetProgressBarValue(int valueToSet)
        {
            if (valueToSet > _maxValueForCurrentSection)
            {
                progressBar.Value = _maxValueForCurrentSection;
            }
            else
            {
                progressBar.Value = valueToSet;
            }
        }

        public void CloseForm()
        {
            RunOnUIThread(() => this.Dispose());
            _isClosing = true;
        }

        public void AddDetails(string output)
        {
            RunOnUIThread(() => 
            {
                if (detailsTextBox.TextLength > 0)
                {
                    detailsTextBox.AppendText(Environment.NewLine);
                }
                detailsTextBox.AppendText(output);
            });

        }

        public void ClearDetails()
        {
            RunOnUIThread(() => detailsTextBox.Text = string.Empty);
        }

        public void SetCurrentValue(int valueToSet)
        {
            RunOnUIThread(() => progressBar.Value = valueToSet);
        }

        public void FinalizeExecution()
        {
            RunOnUIThread(() =>
            {
                _timer.Stop();
                this.cancelButton.Text = "OK";
                progressBar.Value = progressBar.Maximum;
            });
        }

        private void RunOnUIThread(Action toRun)
        {
            if (!_isClosing)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    toRun.Invoke();
                });
            }
        }

        public void SetMaxValueForCurrentSection(int maxValueForCurrentSection)
        {
            _maxValueForCurrentSection = maxValueForCurrentSection;
        }
    }
}
