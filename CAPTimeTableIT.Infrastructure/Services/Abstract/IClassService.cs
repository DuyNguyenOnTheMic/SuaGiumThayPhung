using CAPTimeTableIT.Infrastructure.Models;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using CAPTimeTableIT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface IClassService
    {
        Task<List<Class>> GetAllAsync();
        List<Class> GetAll();
        Task<Class> GetByIdAsync(int id);
        List<Class> GetBySemesterId(int semesterId);
        Class GetById(int id);
        int AddRange(IList<Class> entity);
        Task<Class> AddAsync(Class entity);
        Task<int> UpdateAsync(Class entity);
        Task<int> DeleteAsync(Class entity);
        Task<int> DeleteAsync(int semesterId);
        Task<Class> ImportDataFromExcel(ApplicationDbContext db);
        Task<Class> AddCreatedAssignerIdAsync(string createdAssignerId);
        Task<Class> GetByMaLHPAsync(string maLHP);
        Task<Class> GetByThuSAsync(int thuS);
        Task<Class> GetByTietSAsync(int TietS);
        Task<Class> GetByMaLHPAndThuSAsync(int semesterId, string maLHP, int thuS);
        Task<Class> GetByTeacherIdAndThuSAsync(int semesterId, int thuS, int tietS, string teacherId);
        Task<ValidationResult> ValidateTeacherInAClassAsync(string teacherId, int currentClassId);
        Task<byte[]> ExportTimeTableAsync(int semesterId);
    }
}
