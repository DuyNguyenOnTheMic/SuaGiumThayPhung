using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface ISubjectService
    {
        Task<List<Subject>> GetAllAsync();
        List<Subject> GetAll();
        Task<Subject> GetByIdAsync(int id);
        Subject GetById(int id);
        Task<Subject> AddAsync(Subject entity);
        Task<int> UpdateAsync();
        Task<List<SubjectSummaryModel>> GetAllSubjectSummaryAsync();
        Task<List<ClassViewModel>> GetAllClassInWeekAsync(int semesterId);
        Task<List<ClassViewModel>> GetAllClassInWeekByClassIdAndTeacherIdAsync(int semesterId, string teacherId);
        Task<Subject> GetBySubjectCodeAsync(string subjectCode);
        Task<Subject> GetByNameAsync(string name);
        Task<Subject> GetByNameOrSubjectCodeAsync(string name, string subjectCode);
    }
}
