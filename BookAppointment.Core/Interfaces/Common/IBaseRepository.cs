using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Common
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<IQueryable<T>> GetAllIncluding(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        Task<T?> GetById(int id);
        void Update(T entity);
        void Insert(T entity);
        void Delete(T entity);
    }
}
