using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.Models
{
    public class UserLoginDetails
    {
        public int UserId { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string SecurityQuestion { get; set; }

        public string SecurityAnswer { get; set; }

        public string UserLoginType { get; set; }

        public string UserLoginValue { get; set; }

        public string LoginMessage { get; set; }

        public string AdminCodeNumber { get; set; }
    }
}
