using CAPTimeTableIT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface IUserProfileService
    {
        Task<List<UserProfile>> GetAllAsync();
        List<UserProfile> GetAll();
        Task<UserProfile> GetByIdAsync(string id);
        UserProfile GetById(string id);
        Task<UserProfile> AddAsync(UserProfile entity);
        Task<int> UpdateAsync(UserProfile entity);
        Task<int> DeleteAsync(UserProfile entity);
        UserProfile FindUserProfile(string staffCode);
    }
}
