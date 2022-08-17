using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class RoleService : IRoleService
    {
        #region fields
        private readonly DbContext _context;
        private readonly DbSet<ApplicationRole> _dbSet;
        //private readonly IRepository<ApplicationRole> _applicationRepository;
        #endregion
        #region ctor
        public RoleService(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<ApplicationRole>();
        }
        #endregion
        #region methods
        public Task<List<ApplicationRole>> GetAllAsync()
        {
            var query = _dbSet.AsQueryable().ToListAsync();

            return query;
        }

        public List<ApplicationRole> GetAll()
        {
            var query = _dbSet.AsQueryable();

            return query.ToList();
        }

        public Task<ApplicationRole> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var result = _dbSet.SqlQuery("select * from AspNetUsers where id = @p0", id).FirstOrDefault();

            return Task.FromResult(result);
        }

        public ApplicationRole GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return _dbSet.SqlQuery("select * from AspNetRoles where id= @p0", id).FirstOrDefault();
        }

        public Task<ApplicationRole> AddAsync(ApplicationRole entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            var newRole = _dbSet.Add(entity);
            _context.SaveChanges();
            return Task.FromResult(newRole);
        }

        public Task<ApplicationRole> UpdateAsync(ApplicationRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> DeleteAsync(ApplicationRole entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

