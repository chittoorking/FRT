using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ELearningPortalMSAzureV1.ConfigurationData
{
    public class GetDBConnectionString
    {
        //23-Jan Copied the Below codes from Muted Video(CRUD Operations in ASP.NET Core MVC).
        public readonly IConfiguration configuration;
        public string connectionString;
        public GetDBConnectionString(IConfiguration config)
        {
            this.configuration = config;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
