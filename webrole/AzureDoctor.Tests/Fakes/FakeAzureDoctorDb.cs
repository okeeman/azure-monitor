// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This is the fake database which is used for unit tests.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added to bring IAzureDoctorDb into scope.
using AzureDoctor.Models;

namespace AzureDoctor.Tests
{
    class FakeAzureDoctorDb : IAzureDoctorDb
    {
        public List<object> Added = new List<object>();
        public List<object> Updated = new List<object>();
        public List<object> Removed = new List<object>();
        public bool Saved = false;
        public Dictionary<Type, object> Sets = new Dictionary<Type, object>();

        public IQueryable<T> Query<T>() where T : class
        {
            return Sets[typeof(T)] as IQueryable<T>;
        }

        public void Dispose()
        {
        }

        public void AddSet<T>(IQueryable<T> objects)
        {
            Sets.Add(typeof(T), objects);
        }

        public void Add<T>(T entity) where T : class
        {
            Added.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            Updated.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            Removed.Add(entity);
        }

        public void SaveChanges()
        {
            Saved = true;
        }
    }
}
