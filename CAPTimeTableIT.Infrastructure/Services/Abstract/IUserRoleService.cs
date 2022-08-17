using CAPTimeTableIT.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface IUserRoleService 
    {
        Task<List<ApplicationUserRole>> GetAllAsync();
        List<ApplicationUserRole> GetAll();
        Task<ApplicationUserRole> GetByIdAsync(string id);
        ApplicationUserRole GetById(string id);
    }
}
