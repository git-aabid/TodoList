using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TODO.Contacts.Repository;

namespace TODO.Repo.Repository
{
    public class GenericRepo<T> : IRepo<T> where T : class
    {
        private readonly ToDoDbContext _dbContext;
        private readonly DbSet<T> _table;
        public GenericRepo(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }

        public async Task<T> Get(object id)
        {
            var res= await _table.FindAsync(id);
            return res;
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            return await _table.Where(filter).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _table;
            return await _table.Where(filter).AsNoTracking().ToListAsync();
        }

        public async Task<int> Insert(T entity)
        {
            await _table.AddAsync(entity);
            return await Save();

        }

        public async Task<int> Update(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
           return await Save();
        }

        public async Task<int> Save()
        {
           return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Remove(Expression<Func<T, bool>> filter)
        {
            var tEntity = await Get(filter);
            _table.Remove(tEntity);
            return await Save();
        }
    }
}
