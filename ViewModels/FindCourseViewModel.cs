using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;

namespace ELearningPortalMSAzureV1.ViewModels
{
    public class FindCourseViewModel : CommonViewModel
    {
        public List<Course> EnrolledCourseList { get; set; }
        public List<Course> AvailableCourseList { get; set; }
        public List<Course> SearchedCourseList { get; set; }
        public Enrollment Enrollment { get; set; }
        public bool IsLearnerProfileComplete { get; set; }
        public string search_Query;
        public bool IsSearcheClicked;
    }
}
