using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.ViewModels;

namespace ELearningPortalMSAzureV1.Areas.Learner.ViewModels
{
    public class LearnerProfileViewModel: CommonViewModel
    {
        public List<Course> CourseList { get; set; }
        public Enrollment Enrollment { get; set; }
        public bool IsLearnerProfileComplete { get; set; }
        public LearnerDetails LearnerDetails { get; set; }
    }
}
