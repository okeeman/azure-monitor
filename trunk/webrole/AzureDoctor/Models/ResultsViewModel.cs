// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This model is used to allow the display of results from two database tables. A 
// may only be based on one model.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureDoctor.Models
{
    public class ResultsViewModel 
    {
        public ResultsViewModel()
        {
            this.InstanceStatuses = new List<string>();
        }

        public string CloudServiceName { get; set; }
        public List<string> InstanceStatuses { get; set; }
        public string DateTime { get; set; }
        public string Location { get; set; }
        public string InstanceCount { get; set; }
        public string InstanceSize { get; set; }
        public string DeploymentSlot { get; set; }
        public string HostedServiceStatus { get; set; }
        public string VIPAddress { get; set; }
    }
}
