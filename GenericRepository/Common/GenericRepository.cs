﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Migrations;

namespace GenericRepository.Common
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity<int>,new()
    {
        DbContext context;
        IDbSet<T> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public T AddOrUpdate(T obj)
        {
            dbSet.AddOrUpdate(x => x.Id, obj);
            Save();
            return obj;
        }

        public T Delete(T obj)
        {
            T temp = dbSet.Find(obj.Id);
            dbSet.Remove(temp);
            Save();
            return obj;
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet;
        }

        public void DeleteRange(IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                T temp = dbSet.Find(item.Id);
                dbSet.Remove(temp);
            }
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
