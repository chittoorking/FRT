using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.ViewModels;

namespace ELearningPortalMSAzureV1.Areas.Admin.ViewModels
{
    public class AdminProfileViewModel: CommonViewModel
    {
        public bool IsAdminProfileComplete { get; set; }
        public AdminDetails AdminDetails { get; set; }
    }
}
