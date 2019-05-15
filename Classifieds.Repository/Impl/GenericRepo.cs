using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Classifieds.Repository.Impl
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepo<T> where T : class
    {

        private ApplicationContext context;
        private DbSet<T> dbSet;

        public GenericRepo(ApplicationContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
        /// <summary>
        /// Insert object into the database
        /// </summary>
        /// <param name="entity"></param>
        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
        }
        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        /// <summary>
        /// Delete an object from the database
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            T entity = Find(id);

            if (entity != null)
            {
                context.Set<T>().Remove(entity);
            }

        }
        /// <summary>
        /// Delete multiple objects
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(List<T> entities)
        {

            if (entities != null)
            {
                context.Set<T>().RemoveRange(entities);
            }

        }
        /// <summary>
        /// Save changes to the database
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }
        /// <summary>
        /// Retrieve object and children
        /// </summary>
        /// <param name="id">id of entity</param>
        /// <param name="includes">eager statements</param>
        /// <returns></returns>
        public T Find(long id, Expression<Func<T, Object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            foreach (var property in includes)
            {
                query = query.Include(property);
            }

            var propertyName = "ID";
            var parameter = Expression.Parameter(typeof(T), "param");

            //Create lamdba expressions
            var predicateLeft = Expression.PropertyOrField(parameter, propertyName);
            var predicateRight = Expression.Constant(id);
            var predicate = Expression.Lambda<Func<T, bool>>
                (Expression.Equal(predicateLeft, predicateRight), parameter);

            return query.FirstOrDefault(predicate);
        }
        /// <summary>
        /// Retrieve entity using id
        /// </summary>
        /// <param name="id">id of entity</param>
        /// <returns></returns>
        public T Find(long id)
        {
            return context.Set<T>().Find(id);
        }
        /// <summary>
        /// Fetch all objects from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> FindAll()
        {
            return context.Set<T>().ToList();
        }
        /// <summary>
        /// Find using where expression
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> where, 
            Expression<Func<T, Object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();
            T entity = null;

            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }

            if (where != null)
                entity = query.FirstOrDefault(where);

            return entity;
            
        }
        /// <summary>
        /// Fetch all objects, include children and Where stement
        /// </summary>
        /// <param name="wherePredicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> wherePredicate,
            Expression<Func<T, Object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
            { 
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }

            if (wherePredicate != null)
                query = query.Where(wherePredicate);

            return query.ToList();
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
