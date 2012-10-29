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
namespace AzureDoctor
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class AzureDoctorEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new AzureDoctorEntities object using the connection string found in the 'AzureDoctorEntities' section of the application configuration file.
        /// </summary>
        public AzureDoctorEntities() : base("name=AzureDoctorEntities", "AzureDoctorEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new AzureDoctorEntities object.
        /// </summary>
        public AzureDoctorEntities(string connectionString) : base(connectionString, "AzureDoctorEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new AzureDoctorEntities object.
        /// </summary>
        public AzureDoctorEntities(EntityConnection connection) : base(connection, "AzureDoctorEntities")
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
        public ObjectSet<User> Users
        {
            get
            {
                if ((_Users == null))
                {
                    _Users = base.CreateObjectSet<User>("Users");
                }
                return _Users;
            }
        }
        private ObjectSet<User> _Users;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Users EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToUsers(User user)
        {
            base.AddObject("Users", user);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="azuredoctorModel", Name="User")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class User : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new User object.
        /// </summary>
        /// <param name="id">Initial value of the ID property.</param>
        public static User CreateUser(global::System.Int32 id)
        {
            User user = new User();
            user.ID = id;
            return user;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int32 _ID;
        partial void OnIDChanging(global::System.Int32 value);
        partial void OnIDChanged();
    
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

    
    }

    #endregion

    
}