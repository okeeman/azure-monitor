//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkerRole1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InstanceStatu
    {
        public int InstanceStatusID { get; set; }
        public int RequestID { get; set; }
        public string InstanceStatus { get; set; }
    
        public virtual RequestResult RequestResult { get; set; }
    }
}