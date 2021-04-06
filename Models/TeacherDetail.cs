using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ELearningPortalMSAzureV1.Models
{
    public class TeacherDetail
    {
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public string Currentprofession { get; set; }
        public string SubjectsOfInterests { get; set; }
        public string Experience { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentState { get; set; }
        public string HighestQualification { get; set; }
        public string AboutMeDescription { get; set; }
        public IFormFile TeacherImageFile { set; get; }
    }
}
