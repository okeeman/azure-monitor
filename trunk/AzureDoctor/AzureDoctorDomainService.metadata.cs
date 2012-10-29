
namespace AzureDoctor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies UserMetadata as the class
    // that carries additional metadata for the User class.
    [MetadataTypeAttribute(typeof(User.UserMetadata))]
    public partial class User
    {

        // This class allows you to attach custom attributes to properties
        // of the User class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserMetadata()
            {
            }

            public int ID { get; set; }

            public string ServiceName { get; set; }

            public string SubscriptionID { get; set; }

            public string Thumbprint { get; set; }
        }
    }
}
