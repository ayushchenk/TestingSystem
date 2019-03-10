﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GenericRepository.Common
{
    public interface IGenericRepository<T> where T : class, Entity<int>, new ()
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Get(int id);
        T AddOrUpdate(T obj);
        T Delete(T obj);
        void Save();
    }
}
