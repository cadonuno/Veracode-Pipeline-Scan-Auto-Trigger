using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Veracode_Pipeline_Scan_Auto_Trigger.globals;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class PipelineScanResults
    {
        private List<PipelineScanResultItem> _results = new List<PipelineScanResultItem>();

        private PipelineScanResults(List<PipelineScanResultItem> results)
        {
            _results = new List<PipelineScanResultItem>(results);
        }

        public static Optional<PipelineScanResults> Of(string scanOutputFile)
        {
            ResultsJson.Example resultsJson = JsonConvert.DeserializeObject<ResultsJson.Example>(ReadScanOutputFile(scanOutputFile));

            List<PipelineScanResultItem> loadedItems = new List<PipelineScanResultItem>();
            foreach (ResultsJson.Finding finding in resultsJson.findings)
            {

                loadedItems.Add(new PipelineScanResultItem()
                {
                    IssueId = finding.issue_id,
                    CweId = finding.cwe_id,
                    Severity = GetSeverityAsString(finding.severity),
                    CweName = finding.issue_type,
                    Details = finding.display_text,
                    FileName = finding.files.source_file.file,
                    Line = finding.files.source_file.line
                });
            }

            return Optional<PipelineScanResults>.Of(new PipelineScanResults(loadedItems));
        }

        private static string GetSeverityAsString(int severity)
        {
            switch(severity)
            {
                case 1:
                    return "Informational";
                case 2:
                    return "Low";
                case 3:
                    return "Medium";
                case 4:
                    return "High";
                case 5:
                    return "Very High";
            }
            return "";
        }

        private static string ReadScanOutputFile(string scanOutputFile)
        {
            return File.ReadAllText(scanOutputFile);
        }

        public List<PipelineScanResultItem> GetAsList()
        {
            return new List<PipelineScanResultItem>(_results);
        }
    }
}
