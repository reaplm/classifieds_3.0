using Classifieds.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Classifieds.Service.Impl
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private IGenericRepo<T> genericRepo;

        public GenericService(IGenericRepo<T> genericRepo)
        {
            this.genericRepo = genericRepo;
        }

        public void Create(T entity)
        {
            genericRepo.Create(entity);
        }
        public void Update(T entity)
        {
            genericRepo.Update(entity);
        }
        public int Delete(long id)
        {
            return genericRepo.Delete(id);

        }
        public void Save()
        {
            genericRepo.Save();
        }
        public T Find(long id)
        {
            return genericRepo.Find(id);
        }
        public T Find(Expression<Func<T, bool>> where,
            Expression<Func<T, Object>>[] includes)
        {
            return genericRepo.Find(where, includes);
        }
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> wherePredicate,
            Expression<Func<T, Object>>[] includes)
        {
            return genericRepo.FindAll(wherePredicate, includes);
        }
        public IEnumerable<T> FindAll()
        {
            return genericRepo.FindAll();
        }
        public T Find(long id, Expression<Func<T, Object>>[] includes)
        {
            return genericRepo.Find(id, includes);
        }
        public int Update(T entity, Object[] keyValues, string[] includes)
        {
            return genericRepo.Update(entity, keyValues, includes);
        }
    }
}
