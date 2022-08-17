using CAPTimeTableIT.Domain;
using System;
using System.Collections.Generic;
using CAPTimeTableIT.Infrastructure.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPTimeTableIT.Infrastructure.Models.Classes;
using CAPTimeTableIT.Infrastructure.Models.Semesters;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface ISemesterService
    {
        Task<List<Semester>> GetAllAsync();
        List<Semester> GetAll();
        Task<List<SemesterSummary>> GetSemesterSummaries();
        Task<Semester> GetByIdAsync(int id);
        Semester GetById(int id);
        Semester GetTheLatestSemester();
        Task<Semester> AddAsync(Semester entity);
        Task<int> UpdateAsync();
        bool Delete(int id);
        Task<ValidationResult> ValidateDeleteSemesterIdAsync(int semesterId);
        /*Task AddAsync(Models.SemesterAddViewModel newSemester);*/
        Semester FindSemester(string semester);
        CountClassModel CountClass(int semesterId);
    }
}
