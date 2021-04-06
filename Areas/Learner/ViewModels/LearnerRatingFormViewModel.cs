using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.ViewModels;

namespace ELearningPortalMSAzureV1.Areas.Learner.ViewModels
{
    public class LearnerRatingFormViewModel: CommonViewModel
    {
        public bool IsLearnerProfileComplete { get; set; }
        public Course CurrentCourse { get; set; }
        public CourseRating CourseRating { get; set; }
    }
}
