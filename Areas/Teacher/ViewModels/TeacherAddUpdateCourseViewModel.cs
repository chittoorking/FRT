using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;
using ELearningPortalMSAzureV1.ViewModels;

namespace ELearningPortalMSAzureV1.ViewModels
{
    public class TeacherAddUpdateCourseViewModel : CommonViewModel
    {
        public Course Course { get; set; }
        public int TeacherId { get; set; }
    }
}
