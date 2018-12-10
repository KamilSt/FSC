using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FSC.DataLayer.Repository
{
    public interface IDataRepository
    { }

    public interface IDataRepository<T> : IDataRepository
        where T : class, new()
    {
        T Add(T entity);

        void Remove(T entity);

        void Remove(int id);

        T Update(T entity);

        IQueryable<T> Get();

        T Get(int id);
    }
}