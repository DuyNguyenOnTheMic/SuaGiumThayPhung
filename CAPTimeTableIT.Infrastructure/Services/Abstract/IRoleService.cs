using CAPTimeTableIT.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface IRoleService
    {
        Task<List<ApplicationRole>> GetAllAsync();
        List<ApplicationRole> GetAll();
        Task<ApplicationRole> GetByIdAsync(string id);
        ApplicationRole GetById(string id);
        Task<ApplicationRole> AddAsync(ApplicationRole entity);
        Task<ApplicationRole> UpdateAsync(ApplicationRole entity);
        Task<ApplicationRole> DeleteAsync(ApplicationRole entity);
    }
}
