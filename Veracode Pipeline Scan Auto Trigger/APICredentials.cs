using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger
{
    public class APICredentials
    {
        private string _veracodeApiId;
        private string _veracodeApiKey;

        public APICredentials(string veracodeApiId, string veracodeApiKey)
        {
            _veracodeApiId = veracodeApiId;
            _veracodeApiKey = veracodeApiKey;
        }

        public string VeracodeApiId { get => _veracodeApiId; }
        public string VeracodeApiKey { get => _veracodeApiKey; }

        public static APICredentials FromSettings(GlobalOptions globalOptions)
        {
            if (globalOptions.CredentialsType == CredentialsTypeEnum.LiteralCredentials)
            {
                return new APICredentials(globalOptions.ApiId, globalOptions.ApiKey);
            }

            string apiId = null;
            string apiKey = null;
            bool foundProfile = false;
            string profileToFind = globalOptions.CredentialsProfileName ?? "default";
            foreach (string line in System.IO.File.ReadLines(GetCredentialsFilePath()))
            {
                string trimmedLine = line.Trim();
                if (foundProfile)
                {
                    if (trimmedLine.StartsWith("veracode_api_key_id"))
                    {
                        apiId = trimmedLine.Substring(trimmedLine.IndexOf("=") + 1);
                    }
                    else if (line.StartsWith("veracode_api_key_secret"))
                    {
                        apiKey = trimmedLine.Substring(trimmedLine.IndexOf("=") + 1);
                    }
                    if (apiId != null && apiKey != null)
                    {
                        break;
                    }
                }
                else if (trimmedLine.StartsWith("["))
                {
                    foundProfile = trimmedLine.Replace("[", "").Replace("]", "").Trim() == profileToFind;
                }
            }
            return new APICredentials(apiId, apiKey);

        }

        private static string GetCredentialsFilePath()
        {
            string credentialsFilePath = Path.Combine(GetHomePath(), ".veracode", "credentials");
            if (!File.Exists(credentialsFilePath))
            {
                //TODO: mark as error
            }
            return credentialsFilePath;
        }

        private static string GetHomePath()
        {
            return (Environment.OSVersion.Platform == PlatformID.Unix ||
                    Environment.OSVersion.Platform == PlatformID.MacOSX)
                    ? Environment.GetEnvironmentVariable("HOME")
                    : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }
    }
}
