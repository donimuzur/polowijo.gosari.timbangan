using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.IServices
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetGenericRepository<T>()
           where T : class;
        void Dispose();
        /// <summary>
        /// Saves current context changes.
        /// </summary>
        void SaveChanges();
        void RevertChanges();
    }
}
