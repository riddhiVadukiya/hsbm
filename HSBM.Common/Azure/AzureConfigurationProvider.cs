using Microsoft.Azure;
using HSBM.Common.Contracts;

namespace HSBM.Common.Azure
{
    public class AzureConfigurationProvider : IConfigurationProvider
    {
        public string GetConfigurationValue(string key)
        {
            return CloudConfigurationManager.GetSetting(key);
        }
    }
}
