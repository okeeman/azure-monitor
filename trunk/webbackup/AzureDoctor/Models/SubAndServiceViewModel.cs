using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureDoctor.Models
{
    public class SubAndServiceViewModel
    {
        public Subscription sub { get; set; }
        public List<CloudService> cloudServices { get; set; }

        public SubAndServiceViewModel()
        {
            this.sub = new Subscription();
            this.cloudServices = new List<CloudService>();
        }
    }
}