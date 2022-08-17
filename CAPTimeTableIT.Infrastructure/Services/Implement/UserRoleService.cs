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
    public class UserRoleService : IUserRoleService
    {
        #region fields
        private readonly DbContext _context;
        private readonly DbSet<ApplicationUserRole> _dbSet;
        //private readonly IRepository<ApplicationUser> _applicationRepository;
        #endregion
        #region ctor
        public UserRoleService(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<ApplicationUserRole>();
        }
        #endregion
        #region methods
        public Task<List<ApplicationUserRole>> GetAllAsync()
        {
            var query = _dbSet.AsQueryable().ToListAsync();

            return query;
        }

        public List<ApplicationUserRole> GetAll()
        {
            var query = _dbSet.AsQueryable();

            return query.ToList();
        }

        public Task<ApplicationUserRole> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var result = _dbSet.SqlQuery("select * from AspNetUsers where id = @p0", id).FirstOrDefault();

            return Task.FromResult(result);
        }

        public ApplicationUserRole GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return _dbSet.SqlQuery("select * from AspNetUsers where id= @p0", id).FirstOrDefault();
        }
        #endregion
    }
}
