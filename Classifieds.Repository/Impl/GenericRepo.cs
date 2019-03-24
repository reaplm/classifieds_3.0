using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class GenericRepo<T> where T : class
    {

        private ApplicationContext context;
        private DbSet<T> dbSet;

        public GenericRepo(ApplicationContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public void create(T entity)
        {
            context.Set<T>().Add(entity);
        }
        public void update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public void delete(Int64 id)
        {
            T entity = find(id);

            if (entity != null)
            {
                context.Set<T>().Remove(entity);
            }

        }
        public void save()
        {
            context.SaveChanges();
        }
        public T find(Int64 id)
        {
            return context.Set<T>().Find(id);
        }

        public IEnumerable<T> findAll()
        {
            IEnumerable<T> list = dbSet.ToList();
            return list;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
