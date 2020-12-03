using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TODO.Contacts.Repository
{
    public interface IRepo<T> where T : class
    {
        Task<T> Get(object id);
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<int> Insert(T entity);
        Task<int> Update(T entity);
        Task<int> Remove(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);
        Task<int> Save();



    }
}
