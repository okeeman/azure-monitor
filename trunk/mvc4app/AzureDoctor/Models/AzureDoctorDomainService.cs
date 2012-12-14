
namespace AzureDoctor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // Implements application logic using the AzureDoctorEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class AzureDoctorDomainService : LinqToEntitiesDomainService<AzureDoctorEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'AzureDoctorUsers' query.
        public IQueryable<AzureDoctorUser> GetAzureDoctorUsers()
        {
            return this.ObjectContext.AzureDoctorUsers;
        }

        public void InsertAzureDoctorUser(AzureDoctorUser azureDoctorUser)
        {
            if ((azureDoctorUser.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(azureDoctorUser, EntityState.Added);
            }
            else
            {
                this.ObjectContext.AzureDoctorUsers.AddObject(azureDoctorUser);
            }
        }

        public void UpdateAzureDoctorUser(AzureDoctorUser currentAzureDoctorUser)
        {
            this.ObjectContext.AzureDoctorUsers.AttachAsModified(currentAzureDoctorUser, this.ChangeSet.GetOriginal(currentAzureDoctorUser));
        }

        public void DeleteAzureDoctorUser(AzureDoctorUser azureDoctorUser)
        {
            if ((azureDoctorUser.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(azureDoctorUser, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.AzureDoctorUsers.Attach(azureDoctorUser);
                this.ObjectContext.AzureDoctorUsers.DeleteObject(azureDoctorUser);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'CloudServices' query.
        public IQueryable<CloudService> GetCloudServices()
        {
            return this.ObjectContext.CloudServices;
        }

        public void InsertCloudService(CloudService cloudService)
        {
            if ((cloudService.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(cloudService, EntityState.Added);
            }
            else
            {
                this.ObjectContext.CloudServices.AddObject(cloudService);
            }
        }

        public void UpdateCloudService(CloudService currentCloudService)
        {
            this.ObjectContext.CloudServices.AttachAsModified(currentCloudService, this.ChangeSet.GetOriginal(currentCloudService));
        }

        public void DeleteCloudService(CloudService cloudService)
        {
            if ((cloudService.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(cloudService, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.CloudServices.Attach(cloudService);
                this.ObjectContext.CloudServices.DeleteObject(cloudService);
            }
        }
    }
}


