using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FSC.DataLayer.Repository
{
    public abstract class DataRepositoryBase<T> : DataRepositoryBase<T, ApplicationDbContext>
       where T : class, new()
    { }

    public abstract class DataRepositoryBase<T, U> : IDataRepository<T>
        where T : class, new()
        where U : DbContext, new()
    {
        protected abstract T AddEntity(U entityContext, T entity);
        protected abstract T UpdateEntity(U entityContext, T entity);
        protected abstract IQueryable<T> GetEntities(U entityContext);
        protected abstract T GetEntity(U entityContext, int id);

        public T Add(T entity)
        {
            using (U entityContext = new U())
            {
                T addedEntity = AddEntity(entityContext, entity);
                entityContext.SaveChanges();
                return addedEntity;
            }
        }

        public void Remove(T entity)
        {
            using (U entityContext = new U())
            {
                entityContext.Entry<T>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (U entityContext = new U())
            {
                T entity = GetEntity(entityContext, id);
                entityContext.Entry<T>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public T Update(T entity)
        {
            using (U entityContext = new U())
            {
                T existingEntity = UpdateEntity(entityContext, entity);
                entityContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                entityContext.SaveChanges();
                return existingEntity;
            }
        }

        public IQueryable<T> Get()
        {
            using (U entityContext = new U())
                return (GetEntities(entityContext)).AsQueryable();
        }

        public T Get(int id)
        {
            using (U entityContext = new U())
                return GetEntity(entityContext, id);
        }
    }
}