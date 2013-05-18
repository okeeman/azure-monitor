// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This is used to allow data from two database tables to be viewed on one page.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureDoctor.Models
{
    public class SubAndServiceViewModel
    {
        public Subscription Sub { get; set; }
        public List<CloudService> CloudServices { get; set; }

        public SubAndServiceViewModel()
        {
            this.Sub = new Subscription();
            this.CloudServices = new List<CloudService>();
        }
    }
}