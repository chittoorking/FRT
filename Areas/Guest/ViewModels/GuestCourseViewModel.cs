using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;

namespace ELearningPortalMSAzureV1.Areas.Guest.ViewModels
{
    public class GuestCourseViewModel
    {
        public List<Course> AvailableCourseList { get; set; }
        public Course selectedCourseDetails { get; set; }

        public List<Course> SearchedCourseList { get; set; }
        public string search_Query;
        public bool IsSearcheClicked;
    }
}
