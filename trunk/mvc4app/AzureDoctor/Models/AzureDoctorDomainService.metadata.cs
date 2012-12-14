
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

            [Required(ErrorMessage = "Email address must be supplied")]
            [StringLength(255)]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [StringLength(50)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [StringLength(50)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Passsword must be supplied")]
            [StringLength(64, ErrorMessage = "Password may not be longer than 64 characters")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [StringLength(255)]
            [DataType(DataType.PhoneNumber)]
            public string Phone { get; set; }

            public int UserID { get; set; }

            [Required(ErrorMessage = "Username must be supplied")]
            [StringLength(255)]
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

            [Required(ErrorMessage = "Service name must be supplied")]
            [Display(Name = "Service name")]
            [StringLength(50)]
            public string ServiceName { get; set; }

            [Required(ErrorMessage = "Subscription ID must be supplied")]
            [Display(Name = "Subscription ID")]
            [StringLength(50)]
            public string SubscriptionID { get; set; }

            [Required(ErrorMessage = "Thumbprint must be supplied")]
            [StringLength(50)]
            public string Thumbprint { get; set; }

            public int UserID { get; set; }
        }
    }
}
