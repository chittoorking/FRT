using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.ViewModels;

namespace ELearningPortalMSAzureV1.Areas.Admin.ViewModels
{
    public class CourseApprovalViewModel : CommonViewModel
    {
        public List<Course> CourseListForAdminApproval { get; set; }
        public List<Course> AllCourseList { get; set; }
    }
}
