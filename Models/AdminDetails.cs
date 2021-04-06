using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ELearningPortalMSAzureV1.Models
{
    public class AdminDetails
    {
            public int AdminId { get; set; }
            public int UserId { get; set; }
            public string CurrentCity { get; set; }
            public string CurrentState { get; set; }
            public string HighestQualification { get; set; }
            public IFormFile AdminImageFile { set; get; }
    }
}
