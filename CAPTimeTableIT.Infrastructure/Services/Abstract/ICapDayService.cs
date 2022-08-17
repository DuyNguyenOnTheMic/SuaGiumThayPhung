using CAPTimeTableIT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface ICapDayService
    {
        Task<List<CapDay>> GetAllAsync();
        List<CapDay> GetAll();
        Task<CapDay> GetByIdAsync(string id);
        CapDay GetById(string id);
        Task<CapDay> AddAsync(CapDay entity);
        Task<CapDay> UpdateAsync(CapDay entity);
        Task<CapDay> DeleteAsync(CapDay entity);
    }
}
