using System;
using System.Collections.Generic;
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
        void create(T entity);
        /**
         * Retrieve an object from the database with the given primary key
         * @param id
         * @return
         */
        T find(Int64 id);
        /**
         * Save changes made to a persistent object
         * @param transientObject
         * @return
         */
        void update(T entity);
        /**
         * Remove an object from the database
         * @param persistentObject
         * @return
         * @throws Exception 
         */
        void delete(Int64 id);

        /**
         * Retrieve a list of all objects of class T
         * @param clazz
         * @return
         */
        IEnumerable<T> findAll();


        /**
         * Save changes to the database
         * 
         */
        void save();
    }
}
