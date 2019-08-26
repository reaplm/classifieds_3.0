using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Classifieds.Repository
{
    public interface IGenericRepo<T> where T : class
    {
        /**
        * Persist a new object into the database
        * @param newInstance
        * @return
        */
        void Create(T entity);
        /**
         * Retrieve an object from the database with the given primary key
         * @param id
         * @return
         */
        T Find(long id);
        /**
         * Retrieve object, include child entities
         * @param id of entity
         * @param includes linq eager fetch statements
         */
        T Find(long id, Expression<Func<T, Object>>[] includes);
        /**
         * Save changes made to a persistent object
         * @param transientObject
         * @return
         */
        T Find(Expression<Func<T, bool>> where,
           Expression<Func<T, Object>>[] includes);
        void Update(T entity);
        /**
         * Remove an object from the database
         * @param persistentObject
         * @return
         * @throws Exception 
         */
        int Delete(long id);
        void DeleteRange(List<T> entities);
        /**
         * Retrieve a list of all objects of class T
         * @param clazz
         * @return
         */
        IEnumerable<T> FindAll();
        /**
          * Retrieve all objects, include child entities and Where statement
          * @param id of entity
          * @param includes eager fetch statements
          */
        IEnumerable<T> FindAll(Expression<Func<T, bool>> wherePredicate,
            Expression<Func<T, Object>>[] includes);

        /**
         * Save changes to the database
         * 
         */
        void Save();
        int Update(T entity, Object[] keyValues, string[] includes);
    }
}
