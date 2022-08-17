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
    public class CapDayService : ICapDayService
    {
        #region fields
        private readonly DbContext _context;
        private readonly DbSet<CapDay> _dbSet;
        //private readonly IRepository<ApplicationRole> _applicationRepository;
        #endregion
        #region ctor
        public CapDayService(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<CapDay>();
        }
        #endregion
        #region methods
        public Task<List<CapDay>> GetAllAsync()
        {
            var query = _dbSet.AsQueryable().ToListAsync();

            return query;
        }

        public List<CapDay> GetAll()
        {
            var query = _dbSet.AsQueryable();

            return query.ToList();
        }

        public Task<CapDay> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var result = _dbSet.SqlQuery("select * from CapDay where id = @p0", id).FirstOrDefault();

            return Task.FromResult(result);
        }

        public CapDay GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return _dbSet.SqlQuery("select * from CapDay where id= @p0", id).FirstOrDefault();
        }

        public Task<CapDay> AddAsync(CapDay entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            var newRole = _dbSet.Add(entity);
            _context.SaveChanges();
            return Task.FromResult(newRole);
        }

        public Task<CapDay> UpdateAsync(CapDay entity)
        {
            throw new NotImplementedException();
        }

        public Task<CapDay> DeleteAsync(CapDay entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
