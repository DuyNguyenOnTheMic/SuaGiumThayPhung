using CAPTimeTableIT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface ICapWeekService
    {
        Task<List<CapWeek>> GetAllAsync();
        List<CapWeek> GetAll();
        Task<CapWeek> GetByIdAsync(string id);
        CapWeek GetById(string id);
        Task<CapWeek> AddAsync(CapWeek entity);
        Task<CapWeek> UpdateAsync(CapWeek entity);
        Task<CapWeek> DeleteAsync(CapWeek entity);
    }
}
