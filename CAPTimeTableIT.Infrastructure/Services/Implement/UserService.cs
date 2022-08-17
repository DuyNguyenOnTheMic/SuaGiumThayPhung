using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Models;
using CAPTimeTableIT.Infrastructure.Models.Users;
using CAPTimeTableIT.Infrastructure.Repositories;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class UserService: IUserService
    {

        #region fields
        private readonly DbContext _context;
        private readonly DbSet<ApplicationUser> _dbSet;
        //private readonly IRepository<ApplicationUser> _applicationRepository;
        #endregion
        #region ctor
        public UserService(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<ApplicationUser>();
        }
        #endregion
        #region methods
        public Task<List<CapstoneUser>> GetAllAsync()
        {
            return _context.Database.SqlQuery<CapstoneUser>(@"SELECT u.Id UserId, u.Email, ISNULL(up.FullName, u.UserName) UserName, 
                up.StaffCode
                FROM [dbo].[AspNetUsers] u 
	                LEFT JOIN UserProfiles up ON u.Id = up.Id").ToListAsync();
        }

        public Task<List<CapstoneUser>> GetAllBySemesterIdAsync(int semesterId)
        {
            return _context.Database.SqlQuery<CapstoneUser>($@"SELECT u.Id UserId, u.Email, ISNULL(up.FullName, u.UserName) UserName, 
                up.StaffCode
                FROM [dbo].[AspNetUsers] u 
                INNER JOIN Classes c ON u.Id = c.Teacherid
	                LEFT JOIN UserProfiles up ON u.Id = up.Id
	                WHERE c.SemesterId = {semesterId}
	                GROUP BY u.Id, u.Email,up.FullName, u.UserName, up.StaffCode").ToListAsync();
        }

        public List<CapstoneUser> GetAll()
        {
            return _context.Database.SqlQuery<CapstoneUser>(@"SELECT u.Id UserId, u.Email, ISNULL(up.FullName, u.UserName) UserName, 
                up.StaffCode
                FROM [dbo].[AspNetUsers] u 
	                LEFT JOIN UserProfiles up ON u.Id = up.Id").ToList();
        }

        public Task<UserRoleModel> GetByIdAsync(string id)
        {
            return _context.Database.SqlQuery<UserRoleModel>($@"  SELECT top 1 u.Id UserId, u.Email, ISNULL(up.FullName, u.UserName) UserName, r.Name RoleName, r.Id RoleId FROM [dbo].[AspNetUsers] u 
	LEFT JOIN UserProfiles up ON u.Id = up.Id
    LEFT JOIN[dbo].[AspNetUserRoles] anur ON u.Id = anur.UserId
    LEFT JOIN[dbo].[AspNetRoles] r ON anur.RoleId = r.Id
where u.Id = '{id}'").FirstOrDefaultAsync();
        }

        public ApplicationUser GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return _dbSet.SqlQuery("select * from AspNetUsers where id= @p0", id).FirstOrDefault();
        }

        public List<UserRoleModel> GetUserRoles()
        {
            //var query = from u in 

            return _context.Database.SqlQuery<UserRoleModel>(@"SELECT u.Id UserId, u.Email, ISNULL(up.FullName, u.UserName) UserName, 
                up.StaffCode,
                r.Name RoleName, r.Id RoleId 
                FROM [dbo].[AspNetUsers] u 
	                LEFT JOIN UserProfiles up ON u.Id = up.Id
                    LEFT JOIN[dbo].[AspNetUserRoles] anur ON u.Id = anur.UserId
    LEFT JOIN[dbo].[AspNetRoles] r ON anur.RoleId = r.Id").ToList();
        }

        public Task<ApplicationUser> AddAsync(ApplicationUser entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            var newRole = _dbSet.Add(entity);
            _context.SaveChanges();
            return Task.FromResult(newRole);
        }

        public Task<ApplicationUser> UpdateAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(ApplicationUser entity)
        {
            //remove 2 rows: 1 in user profiles and 1 in asp.net users
            //otherwise: should check UserRoles and delete them
            if(entity == null)
            {
                return 0;
            }
            var userIdParam = new SqlParameter("@userId", entity.Id);
            var result = await _context.Database.SqlQuery<int>("[dbo].[usp_RemoveUser] @userId", userIdParam).FirstOrDefaultAsync();
            return result;
        }

        public Task<ApplicationUser> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var result = _dbSet.SqlQuery("select * from AspNetUsers where Email = @p0", email).FirstOrDefault();

            return Task.FromResult(result);
        }

        public Task<UserRoleModel> GetUserInfoByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;
            var query = string.Format(@"select u.Id UserId, u.Email, u.UserName, r.Id as RoleId, r.Name AS RoleName from AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN aspnetRoles r ON ur.RoleId = r.Id
 where Email = '{0}'", email);
            var result = _context.Database.SqlQuery<UserRoleModel>(query).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ValidationResult> ValidateDeleteUserAsync(string userId)
        {
            var result = new ValidationResult
            {
                Success = true,
                HasConfirm = false,
            };
            var query = $"SELECT top 1 1 FROM [dbo].[Classes] c WHERE c.TeacherId = '{userId}'";
            var countClasses = await _context.Database.SqlQuery<int>(query).FirstOrDefaultAsync();
            if (countClasses > 0)
            {
                result.Success = false;
                var user = await GetByIdAsync(userId);
                result.Message = "Tài khoản " + user.UserName + " đã được phân công. Bạn không thể xóa";
            }
            return result;
        }
        #endregion
    }
}
