using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestingSystem.BOL.Service
{
    public interface IEntityService<TDTO> where TDTO : class, new()
    {
        IEnumerable<TDTO> GetAll();
        IEnumerable<TDTO> FindBy(Expression<Func<TDTO, bool>> predicate);
        TDTO Get(int id);
        TDTO AddOrUpdate(TDTO obj);
        TDTO Delete(TDTO obj);
    }
}
