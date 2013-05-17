// ----------------------------------------------------------------------------------
// Owen Sweeney 2013// 
// ----------------------------------------------------------------------------------
// This is the domain database. It is separate from the physical database. This 
// allows for other data sources to be used instead by changing the connection
// string.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// Added to bring DbContext into scope.
using System.Data.Entity;

namespace WorkerRole1.Models
{
    public interface IAzureDoctorDb : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;

        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void SaveChanges();
    }

    public class AzureDoctorDb : DbContext, IAzureDoctorDb
    {
        public AzureDoctorDb()
            : base("name=AzureDocEntities")
        {
        }

        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<RequestResult> RequestResults { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<CloudService> CloudServices { get; set; }
        public DbSet<InstanceStatu> InstanceStatuses { get; set; }

        IQueryable<T> IAzureDoctorDb.Query<T>()
        {
            return Set<T>();
        }

        void IAzureDoctorDb.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void IAzureDoctorDb.Update<T>(T entity)
        {
            Entry(entity).State = System.Data.EntityState.Modified;
        }

        void IAzureDoctorDb.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        void IAzureDoctorDb.SaveChanges()
        {
            SaveChanges();
        }
    }
}