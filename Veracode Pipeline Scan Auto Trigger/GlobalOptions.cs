using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    //TODO: turn this into a custom view:
    // - set the credentials to password fields
    // - make the credentials invisible when credentialsType is file
    // - make the profile invisible when credentialsType is literal
    public class GlobalOptions : DialogPage
    {

        [Category("Severities to fail scan")]
        [DisplayName("Very High")]
        [Description("Should fail the pipeline scan on Very High flaws")]
        public bool ShouldFailOnVeryHigh { get; set; } = true;

        [Category("Severities to fail scan")]
        [DisplayName("High")]
        [Description("Should fail the pipeline scan on High flaws")]
        public bool ShouldFailOnHigh { get; set; } = true;

        [Category("Severities to fail scan")]
        [DisplayName("Medium")]
        [Description("Should fail the pipeline scan on Medium flaws")]
        public bool ShouldFailOnMedium { get; set; } = false;

        [Category("Severities to fail scan")]
        [DisplayName("Low")]
        [Description("Should fail the pipeline scan on Low flaws")]
        public bool ShouldFailOnLow { get; set; } = false;

        [Category("Severities to fail scan")]
        [DisplayName("Informational")]
        [Description("Should fail the pipeline scan on Informational flaws")]
        public bool ShouldFailOnInformational { get; set; } = false;

        [Category("API Credentials")]
        [DisplayName("Where are the credentials stored?")]
        [Description("Credentials can be loaded from a credentials file or saved in the IDE")]
        public CredentialsTypeEnum CredentialsType { get; set; } = CredentialsTypeEnum.LiteralCredentials;


        [Category("Literal API Credentials")]
        [DisplayName("API ID")]
        [Description("Your Veracode API ID")]
        public string ApiId { get; set; } = "";

        [Category("Literal API Credentials")]
        [DisplayName("API Key")]
        [Description("Your Veracode API Key")]
        [PasswordPropertyText]
        public string ApiKey { get; set; } = "";

        [Category("Credentials File")]
        [DisplayName("Credentials profile name")]
        [Description("Credentials profile name to use - i.e.: default")]
        public string CredentialsProfileName { get; set; } = "default";
    }
}
