using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ELearningPortalMSAzureV1.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public string CourseDuration { get; set; }

        public string CourseDescription { get; set; }

        public bool IsActiveCourse { get; set; }

        public int CourseTeacherId { get; set; }

        public IFormFile CourseImageFile { set; get; }

        public Decimal AverageCourseRating { get; set; }
    }
}
