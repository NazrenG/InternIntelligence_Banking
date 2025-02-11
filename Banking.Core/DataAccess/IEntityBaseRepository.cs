using Banking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.DataAccess
{
    public interface IEntityBaseRepository<T> where T : class, IEntity, new()
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate = null);
        Task<T> GetById(Expression<Func<T, bool>> predicate = null);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
