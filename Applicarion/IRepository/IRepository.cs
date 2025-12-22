using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Applicaion.IRepository
{
    public interface IRepository<T> where T : class
    {

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
          params Expression<Func<T, object>>[] navigationProperties);

        Task<T> GetById(int ob);
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> UpdateAsync(T entity);
        Task<T> RemoveAsync(T entity);

        Task<T> Insertasync(T entity);

        Task InsertRangeAsync(IEnumerable<T> entities);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        Task<bool> Exists(object id);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter=null,
            params Expression<Func<T, object>>[] navigationProperties );

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
           







    }
}
