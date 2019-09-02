using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Classifieds.Repository
{
    public interface IGenericRepo<T> where T : class
    {
        /// <summary>
        /// Create a new object
        /// </summary>
        /// <param name="entity">Object to add to the DB</param>
        void Create(T entity);
        /// <summary>
        /// Retrieve an object from the database with the given primary key
        /// </summary>
        /// <param name="id">The PK of the object</param>
        /// <returns></returns>
        T Find(long id);
        /// <summary>
        /// Retrieve object, include child entities
        /// </summary>
        /// <param name="id">PK of object to retrieve</param>
        /// <param name="includes">array of children to fetch</param>
        /// <returns></returns>
        T Find(long id, Expression<Func<T, Object>>[] includes);
        /// <summary>
        /// Retrieve object using condition
        /// </summary>
        /// <param name="where">Where condition</param>
        /// <param name="includes">Linq statements of childrent to include</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> where,
           Expression<Func<T, Object>>[] includes);
        /// <summary>
        /// Update an object
        /// </summary>
        /// <param name="entity">The object to update</param>
        void Update(T entity);
        /// <summary>
        /// Update object
        /// </summary>
        /// <param name="entity">The object to update</param>
        /// <param name="keyValues">PKs of entiti</param>
        /// <param name="includes">Linq statements of children to include</param>
        /// <returns>Number of rows changed</returns>
        int Update(T entity, Object[] keyValues, string[] includes);
        /// <summary>
        /// Delete an object from the DB
        /// </summary>
        /// <param name="id">PK of the object</param>
        /// <returns>Number of rows changed</returns>
        int Delete(long id);
        /// <summary>
        /// Retrieve all objects of type T
        /// </summary>
        /// <returns>List of objects</returns>
        IEnumerable<T> FindAll();
        /// <summary>
        /// Retrieve all objects of type T
        /// </summary>
        /// <param name="wherePredicate">Where condition</param>
        /// <param name="includes">Linq statements of children to include</param>
        /// <returns></returns>
        IEnumerable<T> FindAll(Expression<Func<T, bool>> wherePredicate,
            Expression<Func<T, Object>>[] includes);
        /// <summary>
        /// Commit changes
        /// </summary>
        void Save();
        
    }
}
