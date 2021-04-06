using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.Models;

namespace ELearningPortalMSAzureV1.ViewModels
{
    public class TutorialsPageViewModel : CommonViewModel
    {
        public List<TutorialFile> TutorialFiles { get; set; }
        public bool IsTeacherProfileComplete { get; set; }
        public Course CurrentCourse { get; set; }
        public string ControllerName { get; set; }
    }
}
