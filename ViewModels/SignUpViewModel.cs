using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;

namespace ELearningPortalMSAzureV1.ViewModels
{
    public class SignUpViewModel
    {
        public Users UserLearner { get; set; }
        public Users UserTeacher { get; set; }
        public Users UserAdmin { get; set; }
        public UserLoginDetails UserLearnerLoginDetails { get; set; }
        public UserLoginDetails UserTeacherLoginDetails { get; set; }
        public UserLoginDetails UserAdminLoginDetails { get; set; }
        public string UserRole { get; set; }
    }
}
