using BookAppointment.Core.Interfaces.Common;
using BookAppointment.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DataContext _context;
        public BaseRepository(DataContext context)
        {
            _context = context;     
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> list;
            try
            {
                list = _context.Set<T>().AsNoTracking();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return list;
        }

        public async Task<IQueryable<T>> GetAllIncluding(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query;
            query = _context.Set<T>().AsNoTracking();

            if (filter != null) query = query.Where(filter);
            string[] array = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array.Length; i++)
            {
                string includeProperty = array[i];
                query = query.Include(includeProperty);
            }
            return orderBy != null ? await Task.FromResult(orderBy(query)) : await Task.FromResult(query);
        }

        public async Task<T?> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Insert(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _context.Set<T>().Remove(entity);
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveTransaction()
        {
            return _context.SaveChangesAsync();
        }
    }
}
