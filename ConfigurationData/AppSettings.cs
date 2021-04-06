using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.ConfigurationData
{
    public class AppSettings
    {
        public string StorageConnectionString { get; set; }

        public string AzureStorageAccountContainer { get; set; }
    }
}
