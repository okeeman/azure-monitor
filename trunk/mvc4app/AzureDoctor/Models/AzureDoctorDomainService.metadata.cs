
namespace AzureDoctor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies AzureDoctorUserMetadata as the class
    // that carries additional metadata for the AzureDoctorUser class.
    [MetadataTypeAttribute(typeof(AzureDoctorUser.AzureDoctorUserMetadata))]
    public partial class AzureDoctorUser
    {

        // This class allows you to attach custom attributes to properties
        // of the AzureDoctorUser class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class AzureDoctorUserMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private AzureDoctorUserMetadata()
            {
            }

            public EntityCollection<CloudService> CloudServices { get; set; }

            public string Email { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Password { get; set; }

            public string Phone { get; set; }

            public int UserID { get; set; }

            public string Username { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies CloudServiceMetadata as the class
    // that carries additional metadata for the CloudService class.
    [MetadataTypeAttribute(typeof(CloudService.CloudServiceMetadata))]
    public partial class CloudService
    {

        // This class allows you to attach custom attributes to properties
        // of the CloudService class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CloudServiceMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CloudServiceMetadata()
            {
            }

            public AzureDoctorUser AzureDoctorUser { get; set; }

            public int CloudServiceID { get; set; }

            public string ServiceName { get; set; }

            public string SubscriptionID { get; set; }

            public string Thumbprint { get; set; }

            public int UserID { get; set; }
        }
    }
}
