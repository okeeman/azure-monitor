//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// Added to bring Required and Display into scope.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureDoctor.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CloudService
    {
        public CloudService()
        {
            this.RequestResults = new HashSet<RequestResult>();
        }
    
        public int CloudServiceID { get; set; }
        public int SubscriptionRecordID { get; set; }
        [Required(ErrorMessage = "Cloud service name is required")]
        [Display(Name = "Cloud service name")]
        public string CloudServiceName { get; set; }
        public string Status { get; set; }
    
        public virtual Subscription Subscription { get; set; }
        public virtual ICollection<RequestResult> RequestResults { get; set; }
    }
}