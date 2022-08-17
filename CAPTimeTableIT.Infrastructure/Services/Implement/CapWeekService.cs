using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class CapWeekService : ICapWeekService
    {
        #region fields
        private readonly DbContext _context;
        private readonly DbSet<CapWeek> _dbSet;
        //private readonly IRepository<ApplicationRole> _applicationRepository;
        #endregion
        #region ctor
        public CapWeekService(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<CapWeek>();
        }
        #endregion
        #region methods
        public Task<List<CapWeek>> GetAllAsync()
        {
            var query = _dbSet.AsQueryable().ToListAsync();

            return query;
        }

        public List<CapWeek> GetAll()
        {
            var query = _dbSet.AsQueryable();

            return query.ToList();
        }

        public Task<CapWeek> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var result = _dbSet.SqlQuery("select * from CapWeek where id = @p0", id).FirstOrDefault();

            return Task.FromResult(result);
        }

        public CapWeek GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return _dbSet.SqlQuery("select * from CapWeek where id= @p0", id).FirstOrDefault();
        }

        public Task<CapWeek> AddAsync(CapWeek entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            var newRole = _dbSet.Add(entity);
            _context.SaveChanges();
            return Task.FromResult(newRole);
        }

        public Task<CapWeek> UpdateAsync(CapWeek entity)
        {
            throw new NotImplementedException();
        }

        public Task<CapWeek> DeleteAsync(CapWeek entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
