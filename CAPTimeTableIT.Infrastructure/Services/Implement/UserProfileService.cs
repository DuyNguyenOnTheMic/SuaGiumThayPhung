using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class UserProfileService: IUserProfileService
    {
        #region fields
        private readonly DbContext _context;
        private readonly DbSet<UserProfile> _dbSet;
        //private readonly IRepository<UserProfile> _applicationRepository;
        #endregion
        #region ctor
        public UserProfileService(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<UserProfile>();
        }
        #endregion
        #region methods
        public Task<List<UserProfile>> GetAllAsync()
        {
            var query = _dbSet.AsQueryable().ToListAsync();

            return query;
        }

        public List<UserProfile> GetAll()
        {
            var query = _dbSet.AsQueryable();

            return query.ToList();
        }

        public Task<UserProfile> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var result = _dbSet.Where(t=>t.Id == id).FirstOrDefault();

            return Task.FromResult(result);
        }

        public UserProfile GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return _dbSet.Where(t => t.Id == id).FirstOrDefault();
        }

        public Task<UserProfile> AddAsync(UserProfile entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            var newRole = _dbSet.Add(entity);
            _context.SaveChanges();
            return Task.FromResult(newRole);
        }

        public Task<int> UpdateAsync(UserProfile entity)
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(UserProfile entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            _dbSet.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public UserProfile FindUserProfile(string staffCode)
        {
            if (string.IsNullOrEmpty(staffCode)) return null;
            var result = _dbSet.FirstOrDefault(t => t.StaffCode == staffCode);
            return result;
        }
        #endregion
    }
}
