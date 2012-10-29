
namespace AzureDoctor
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
    // TODO: add the EnableClientAccessAttribute to this class to expose this DomainService to clients.
    public class AzureDoctorDomainService : LinqToEntitiesDomainService<AzureDoctorEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Users' query.
        public IQueryable<User> GetUsers()
        {
            return this.ObjectContext.Users;
        }

        public void InsertUser(User user)
        {
            if ((user.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(user, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Users.AddObject(user);
            }
        }

        public void UpdateUser(User currentUser)
        {
            this.ObjectContext.Users.AttachAsModified(currentUser, this.ChangeSet.GetOriginal(currentUser));
        }

        public void DeleteUser(User user)
        {
            if ((user.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(user, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Users.Attach(user);
                this.ObjectContext.Users.DeleteObject(user);
            }
        }
    }
}


