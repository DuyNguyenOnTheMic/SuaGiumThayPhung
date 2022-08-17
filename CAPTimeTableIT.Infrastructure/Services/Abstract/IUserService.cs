using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Models;
using CAPTimeTableIT.Infrastructure.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface IUserService
    {
        Task<List<CapstoneUser>> GetAllAsync();
        Task<List<CapstoneUser>> GetAllBySemesterIdAsync(int semesterId);
        List<CapstoneUser> GetAll();
        Task<UserRoleModel> GetByIdAsync(string id);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<UserRoleModel> GetUserInfoByEmailAsync(string email);
        ApplicationUser GetById(string id);
        List<UserRoleModel> GetUserRoles();
        Task<ApplicationUser> AddAsync(ApplicationUser entity);
        Task<ApplicationUser> UpdateAsync(ApplicationUser entity);
        Task<int> DeleteAsync(ApplicationUser entity);
        Task<ValidationResult> ValidateDeleteUserAsync(string userId);
    }
}
