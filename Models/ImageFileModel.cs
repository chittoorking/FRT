using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.Models
{
    public class ImageFileModel
    {
        public int ImageFileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileLocation { get; set; }
        public DateTime? FileUploadDateTime { get; set; }
    }
}
