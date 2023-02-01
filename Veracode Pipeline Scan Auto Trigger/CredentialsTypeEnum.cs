using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public enum CredentialsTypeEnum
    {
        [Description("Credentials File")]
        CredentialsFile,
        [Description("Literal Credentials")]
        LiteralCredentials
    }
}
