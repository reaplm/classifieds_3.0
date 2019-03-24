using Classifieds.Repository;
using System;
using System.Collections.Generic;
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

        public void create(T entity)
        {
            genericRepo.create(entity);
        }
        public void update(T entity)
        {
            genericRepo.update(entity);
        }
        public void delete(Int64 id)
        {
            genericRepo.delete(id);

        }
        public void save()
        {
            genericRepo.save();
        }
        public T find(Int64 id)
        {
            return genericRepo.find(id);
        }
        public IEnumerable<T> findAll()
        {
            return genericRepo.findAll();
        }
    }
}
