using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.ViewModels;

namespace ELearningPortalMSAzureV1.Areas.Learner.ViewModels
{
    public class LearningPageViewModel : CommonViewModel
    {
        public List<TutorialFile> TutorialFiles { get; set; }
        public List<Course> OtherEnrolledCourses { get; set; }
        public bool IsLearnerProfileComplete { get; set; }
        public Course CurrentCourse { get; set; }
    }
}
