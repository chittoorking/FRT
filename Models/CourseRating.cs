using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.Models
{
    public class CourseRating
    {
        public int CourseRatingId { get; set; }
        public int CourseId { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public DateTime? CommentDateTime { get; set; }
        public int RatedByUserId { get; set; }
        public string RatedByUserFullName { get; set; }
        public string RatedByUserNameInitials { get; set; }
    }
}
