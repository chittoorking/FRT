using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;

namespace ELearningPortalMSAzureV1.ViewModels
{
    public class GuestCoursesViewModel
    {
        public Users Users { get; set; }
        public List<Course> CourseList { get; set; }
        public Enrollment Enrollment { get; set; }
    }
}
