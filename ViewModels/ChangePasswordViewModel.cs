using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;

namespace ELearningPortalMSAzureV1.ViewModels
{
    public class ChangePasswordViewModel : CommonViewModel
    {
        public UserLoginDetails UserLoginDetails { get; set; }
        public string ControllerName { get; set; }
    }
}
