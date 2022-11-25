using Human_Resources_Management.ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.REPOSITORIES.Abstract
{
    public interface IGenericRepository<T> where T : BaseClass
    {
        bool Add(T item);
        bool Add(List<T> item);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(int id);
        bool RemoveAll(Expression<Func<T, bool>> exp);
        T GetByID(int id);
        T GetByDefault(Expression<Func<T, bool>> exp);
        List<T> GetActive();
        List<T> GetActive(string[] includes);
        List<T> GetDefault(Expression<Func<T, bool>> exp);
        List<T> GetAll();
        List<T> GetAll(string[] includes);
        bool Activate(int id);
        bool Any(Expression<Func<T, bool>> exp);
        int Save();
    }
}
