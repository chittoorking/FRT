using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;
namespace ELearningPortalMSAzureV1.ViewModels
{
    public class CourseRatingsPageViewModel: CommonViewModel
    {
        public List<CourseRating> courseRatingsList = new List<CourseRating>();
    }
}
