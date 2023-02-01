using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class PipelineScanResultItem
    {
        public int IssueId { get; set; }
        public string Severity { get; set; }
        public string CweId { get; set; }
        public string CweName { get; set; }
        public string Details { get; set; }
        public string FileName { get; set; }
        public int Line { get; set; }
    }
}
