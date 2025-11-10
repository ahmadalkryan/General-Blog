using Applicarion.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T :class 
    {
        private readonly BlogDbContext db;
        private readonly DbSet<T> _set ;
        public Repository(BlogDbContext blogDbContext )
        {
            db = blogDbContext;
            _set = db.Set<T>();
        }

        public async Task<bool> Exists(object id)
        {
            var b = _set.Find(id);

            return b != null;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] navigationProperties)
        {
            //IQueryable<T> query = _set;

            //if (navigationProperties is not null)
            //{
            //    foreach(var navigation in navigationProperties)
            //    {
            //        query = query.Include(navigation);
            //    }
            //    return  await  query.Where(predicate).ToListAsync();
                
            //}
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
          IQueryable<T> set = _set;
            var navs   = db.Model.FindEntityType(typeof(T)).GetNavigations();

           if(navs is not null)
            {
                foreach (var navigation in navs)
                {
                    set = set.Include(navigation.Name);
                }
            }



            // db.SaveChanges();
            return set;
        }
        //generic with include
        public async Task<T> GetById(int ob)
        {
            //var set = await _set.FindAsync(ob);

            var navigationProperties = db.Model.FindEntityType(typeof(T)).GetNavigations();
            var set= _set.AsQueryable();
            foreach (var navigation in navigationProperties)
            {
                set = set.Include(navigation.Name);
            }
            
           // db.SaveChanges();
            return set.FirstOrDefaultAsync(e => EF.Property<int>(e, "ID") == ob).Result;
        }

        public async Task<T> Insertasync(T entity)
        {
           await _set.AddAsync(entity);
           await db.SaveChangesAsync();
            //var nav   = db.Model.FindEntityType(typeof(T)).GetNavigations();
            //foreach (var navigation in nav)
            //{
                
            //}
            //{
                
            //}
            return entity;
        }

        public async Task<T> RemoveAsync(T entity)
        {
              _set.Remove(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
             _set.Update(entity);
           await db.SaveChangesAsync();
            return entity;
        }
    }
}
