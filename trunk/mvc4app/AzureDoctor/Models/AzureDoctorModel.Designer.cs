﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("AzureDoctorModel", "CloudServiceFK1", "AzureDoctorUser", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(AzureDoctor.Models.AzureDoctorUser), "CloudService", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(AzureDoctor.Models.CloudService), true)]

#endregion

namespace AzureDoctor.Models
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class AzureDocEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new AzureDocEntities object using the connection string found in the 'AzureDocEntities' section of the application configuration file.
        /// </summary>
        public AzureDocEntities() : base("name=AzureDocEntities", "AzureDocEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new AzureDocEntities object.
        /// </summary>
        public AzureDocEntities(string connectionString) : base(connectionString, "AzureDocEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new AzureDocEntities object.
        /// </summary>
        public AzureDocEntities(EntityConnection connection) : base(connection, "AzureDocEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<AzureDoctorUser> AzureDoctorUsers
        {
            get
            {
                if ((_AzureDoctorUsers == null))
                {
                    _AzureDoctorUsers = base.CreateObjectSet<AzureDoctorUser>("AzureDoctorUsers");
                }
                return _AzureDoctorUsers;
            }
        }
        private ObjectSet<AzureDoctorUser> _AzureDoctorUsers;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<CloudService> CloudServices
        {
            get
            {
                if ((_CloudServices == null))
                {
                    _CloudServices = base.CreateObjectSet<CloudService>("CloudServices");
                }
                return _CloudServices;
            }
        }
        private ObjectSet<CloudService> _CloudServices;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the AzureDoctorUsers EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToAzureDoctorUsers(AzureDoctorUser azureDoctorUser)
        {
            base.AddObject("AzureDoctorUsers", azureDoctorUser);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the CloudServices EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToCloudServices(CloudService cloudService)
        {
            base.AddObject("CloudServices", cloudService);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="AzureDoctorModel", Name="AzureDoctorUser")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class AzureDoctorUser : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new AzureDoctorUser object.
        /// </summary>
        /// <param name="userID">Initial value of the UserID property.</param>
        /// <param name="firstName">Initial value of the FirstName property.</param>
        /// <param name="lastName">Initial value of the LastName property.</param>
        public static AzureDoctorUser CreateAzureDoctorUser(global::System.Int32 userID, global::System.String firstName, global::System.String lastName)
        {
            AzureDoctorUser azureDoctorUser = new AzureDoctorUser();
            azureDoctorUser.UserID = userID;
            azureDoctorUser.FirstName = firstName;
            azureDoctorUser.LastName = lastName;
            return azureDoctorUser;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                if (_UserID != value)
                {
                    OnUserIDChanging(value);
                    ReportPropertyChanging("UserID");
                    _UserID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("UserID");
                    OnUserIDChanged();
                }
            }
        }
        private global::System.Int32 _UserID;
        partial void OnUserIDChanging(global::System.Int32 value);
        partial void OnUserIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                OnFirstNameChanging(value);
                ReportPropertyChanging("FirstName");
                _FirstName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("FirstName");
                OnFirstNameChanged();
            }
        }
        private global::System.String _FirstName;
        partial void OnFirstNameChanging(global::System.String value);
        partial void OnFirstNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                OnLastNameChanging(value);
                ReportPropertyChanging("LastName");
                _LastName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("LastName");
                OnLastNameChanged();
            }
        }
        private global::System.String _LastName;
        partial void OnLastNameChanging(global::System.String value);
        partial void OnLastNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                OnEmailChanging(value);
                ReportPropertyChanging("Email");
                _Email = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Email");
                OnEmailChanged();
            }
        }
        private global::System.String _Email;
        partial void OnEmailChanging(global::System.String value);
        partial void OnEmailChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                OnPhoneChanging(value);
                ReportPropertyChanging("Phone");
                _Phone = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Phone");
                OnPhoneChanged();
            }
        }
        private global::System.String _Phone;
        partial void OnPhoneChanging(global::System.String value);
        partial void OnPhoneChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Username
        {
            get
            {
                return _Username;
            }
            set
            {
                OnUsernameChanging(value);
                ReportPropertyChanging("Username");
                _Username = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Username");
                OnUsernameChanged();
            }
        }
        private global::System.String _Username;
        partial void OnUsernameChanging(global::System.String value);
        partial void OnUsernameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                OnPasswordChanging(value);
                ReportPropertyChanging("Password");
                _Password = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Password");
                OnPasswordChanged();
            }
        }
        private global::System.String _Password;
        partial void OnPasswordChanging(global::System.String value);
        partial void OnPasswordChanged();

        #endregion

    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("AzureDoctorModel", "CloudServiceFK1", "CloudService")]
        public EntityCollection<CloudService> CloudServices
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<CloudService>("AzureDoctorModel.CloudServiceFK1", "CloudService");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<CloudService>("AzureDoctorModel.CloudServiceFK1", "CloudService", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="AzureDoctorModel", Name="CloudService")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CloudService : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new CloudService object.
        /// </summary>
        /// <param name="cloudServiceID">Initial value of the CloudServiceID property.</param>
        /// <param name="userID">Initial value of the UserID property.</param>
        public static CloudService CreateCloudService(global::System.Int32 cloudServiceID, global::System.Int32 userID)
        {
            CloudService cloudService = new CloudService();
            cloudService.CloudServiceID = cloudServiceID;
            cloudService.UserID = userID;
            return cloudService;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 CloudServiceID
        {
            get
            {
                return _CloudServiceID;
            }
            set
            {
                if (_CloudServiceID != value)
                {
                    OnCloudServiceIDChanging(value);
                    ReportPropertyChanging("CloudServiceID");
                    _CloudServiceID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("CloudServiceID");
                    OnCloudServiceIDChanged();
                }
            }
        }
        private global::System.Int32 _CloudServiceID;
        partial void OnCloudServiceIDChanging(global::System.Int32 value);
        partial void OnCloudServiceIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                OnUserIDChanging(value);
                ReportPropertyChanging("UserID");
                _UserID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("UserID");
                OnUserIDChanged();
            }
        }
        private global::System.Int32 _UserID;
        partial void OnUserIDChanging(global::System.Int32 value);
        partial void OnUserIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String SubscriptionID
        {
            get
            {
                return _SubscriptionID;
            }
            set
            {
                OnSubscriptionIDChanging(value);
                ReportPropertyChanging("SubscriptionID");
                _SubscriptionID = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("SubscriptionID");
                OnSubscriptionIDChanged();
            }
        }
        private global::System.String _SubscriptionID;
        partial void OnSubscriptionIDChanging(global::System.String value);
        partial void OnSubscriptionIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Thumbprint
        {
            get
            {
                return _Thumbprint;
            }
            set
            {
                OnThumbprintChanging(value);
                ReportPropertyChanging("Thumbprint");
                _Thumbprint = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Thumbprint");
                OnThumbprintChanged();
            }
        }
        private global::System.String _Thumbprint;
        partial void OnThumbprintChanging(global::System.String value);
        partial void OnThumbprintChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String ServiceName
        {
            get
            {
                return _ServiceName;
            }
            set
            {
                OnServiceNameChanging(value);
                ReportPropertyChanging("ServiceName");
                _ServiceName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("ServiceName");
                OnServiceNameChanged();
            }
        }
        private global::System.String _ServiceName;
        partial void OnServiceNameChanging(global::System.String value);
        partial void OnServiceNameChanged();

        #endregion

    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("AzureDoctorModel", "CloudServiceFK1", "AzureDoctorUser")]
        public AzureDoctorUser AzureDoctorUser
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<AzureDoctorUser>("AzureDoctorModel.CloudServiceFK1", "AzureDoctorUser").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<AzureDoctorUser>("AzureDoctorModel.CloudServiceFK1", "AzureDoctorUser").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<AzureDoctorUser> AzureDoctorUserReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<AzureDoctorUser>("AzureDoctorModel.CloudServiceFK1", "AzureDoctorUser");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<AzureDoctorUser>("AzureDoctorModel.CloudServiceFK1", "AzureDoctorUser", value);
                }
            }
        }

        #endregion

    }

    #endregion

    
}