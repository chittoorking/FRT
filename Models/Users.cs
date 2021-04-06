using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserRole { get; set; }
        public int UserRoleId { get; set; }
        public string UserFullName { get; set; }
    }
}
