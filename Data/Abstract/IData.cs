using Data.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Data.Abstract
{
    public interface IData<T>
    {
        DataResult<T> Insert(T t);
        DataResult InsertBulk(List<T> ts);
        DataResult<T> Update(T t);
        DataResult<bool> Delete(T t);
        DataResult<bool> DeleteByKey(long id);
        T GetByKey(long id);
        List<T> GetAll();
        int GetCount();

    }
}
