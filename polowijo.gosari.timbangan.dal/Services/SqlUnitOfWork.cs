using polowijo.gosari.timbangan.dal.IServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.Services
{
    public class SqlUnitOfWork : IDisposable, IUnitOfWork
    {
        public SqlUnitOfWork()
        {
            
        }
        //load a context automatically
        private TIMBANGANEntities _context = new TIMBANGANEntities();
        public IGenericRepository<T> GetGenericRepository<T>()
            where T : class
        {
            return new SqlGenericRepository<T>(_context); ;
        }
        
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch(Exception )
            {
                throw;
            }
        }

        public void RevertChanges()
        {
            //overwrite the existing context with a new, fresh one to revert all the changes
            _context = new TIMBANGANEntities();
        }
        /// <summary>
        /// Logs the entity validation errors.
        /// </summary>
        /// <param name="entityValidationErrors">The entity validation errors.</param>
      
        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// the dispose method is called automatically by the injector depending on the lifestyle
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
