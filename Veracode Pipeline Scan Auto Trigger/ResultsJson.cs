using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class ResultsJson
    {
        public class Root
        {
            public string href { get; set; }
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class Help
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Root root { get; set; }
            public Self self { get; set; }
            public Help help { get; set; }
        }

        public class SourceFile
        {
            public string file { get; set; }
            public int line { get; set; }
            public string function_name { get; set; }
            public string qualified_function_name { get; set; }
            public string function_prototype { get; set; }
            public string scope { get; set; }
        }

        public class Files
        {
            public SourceFile source_file { get; set; }
        }

        public class FlawMatch
        {
            public string procedure_hash { get; set; }
            public string prototype_hash { get; set; }
            public string flaw_hash { get; set; }
            public int flaw_hash_count { get; set; }
            public int flaw_hash_ordinal { get; set; }
            public string cause_hash { get; set; }
            public int cause_hash_count { get; set; }
            public int cause_hash_ordinal { get; set; }
            public string cause_hash2 { get; set; }
            public string cause_hash2_ordinal { get; set; }
        }

        public class StackDump
        {
        }

        public class StackDumps
        {
            public IList<StackDump> stack_dump { get; set; }
        }

        public class Finding
        {
            public string title { get; set; }
            public int issue_id { get; set; }
            public string gob { get; set; }
            public int severity { get; set; }
            public string issue_type_id { get; set; }
            public string issue_type { get; set; }
            public string cwe_id { get; set; }
            public string display_text { get; set; }
            public Files files { get; set; }
            public FlawMatch flaw_match { get; set; }
            public StackDumps stack_dumps { get; set; }
            public string flaw_details_link { get; set; }
        }

        public class Example
        {
            public Links _links { get; set; }
            public string scan_id { get; set; }
            public string scan_status { get; set; }
            public string message { get; set; }
            public IList<string> modules { get; set; }
            public int modules_count { get; set; }
            public IList<Finding> findings { get; set; }
            public string pipeline_scan { get; set; }
            public string dev_stage { get; set; }
        }

    }
}
