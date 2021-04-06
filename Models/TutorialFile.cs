using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.Models
{
    public class TutorialFile
    {
        public string TutorialName { get; set; }
        public int CourseId { get; set; }
        public int FileId { get; set; }
        public int SequenceId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileLocation { get; set; }
        public string FileUploadDateTime { get; set; }
    }
}
