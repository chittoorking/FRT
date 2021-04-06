using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.Models
{
    public class Enrollment
    {

        public int EnrollmentID { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string LearnerId { get; set; }
        public DateTime EnrollmentDate { get; set; }


    }
}
