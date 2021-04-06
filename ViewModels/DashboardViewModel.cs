using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningPortalMSAzureV1.ViewModels
{
    public class DashboardViewModel : CommonViewModel
    {
        public List<ActionDetails> ActionDetailsList;
        public string DashboardHeader { get; set; }
        public DashboardViewModel()
        {
            ActionDetailsList = new List<ActionDetails>();
        }

    }
    public class ActionDetails
    {
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string DisplayName { get; set; }
    }
}
