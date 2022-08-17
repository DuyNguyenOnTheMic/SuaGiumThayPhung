using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using CAPTimeTableIT.Infrastructure.Repositories;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class SubjectService : ISubjectService
    {

        #region fields
        private readonly DbContext _context;
        private readonly DbSet<Subject> _dbSet;
        private readonly DbSet<Class> _classes;
        private readonly DbSet<Semester> _semesters;
        private readonly IRepository<Subject> _subjectRepository;
        #endregion
        #region ctor
        public SubjectService(DbContext context, IRepository<Subject> subjectRepository)
        {
            _context = context;
            _dbSet = _context.Set<Subject>();
            _classes = _context.Set<Class>();
            _subjectRepository = subjectRepository;
            _semesters = _context.Set<Semester>();
        }
        #endregion
        #region methods
        public Task<List<Subject>> GetAllAsync()
        {
            var query = _dbSet.AsQueryable().ToListAsync();

            return query;
        }

        public List<Subject> GetAll()
        {
            var query = _subjectRepository.GetAll();

            return query.ToList();
        }
        /*public List<Subject> GetByName()
        {
            var subjectName = _subjectRepository.GetAll();
            var foundItem = subjectName.Select(s => new { s.Name }).ToList();

            return foundItem;
        }*/

        public Task<Subject> GetByIdAsync(int id)
        {
            if (id == 0)
                return null;

            var result = _subjectRepository.Get(id);

            return Task.FromResult(result);
        }

        public Subject GetById(int id)
        {
            if(id == 0)
                return null;

            var result = _subjectRepository.Get(id);

            return result;
        }

        public Task<List<SubjectSummaryModel>> GetAllSubjectSummaryAsync()
        {
            var query = _dbSet.AsQueryable().Select(t=>new SubjectSummaryModel
            {
                Id = t.Id,
                Name = t.Name
            }).OrderBy(t=>t.Name).ToListAsync();

            return query;
        }

        public Task<List<ClassViewModel>> GetAllClassInWeekAsync(int semesterId)
        {
            var classModelQuery = (from se in _semesters
                                   join c in _classes on se.Id equals c.SemesterId
                                   join s in _dbSet on c.SubjectId equals s.Id                          
                                   where se.Id == semesterId
                                   orderby c.ThuS, c.TietS, s.Name
                                   select new ClassViewModel
                                   {
                                       SemesterId = semesterId,
                                       ClassId = c.Id,
                                       ThuS = c.ThuS,
                                       TietS = c.TietS,
                                       MaLHP = c.MaLHP,
                                       LoaiHP = c.LoaiHP,
                                       Phong = c.Phong,
                                       TeacherId = c.TeacherId,
                                       TeacherEmail = c.TeacherEmail,
                                       CreatedAssignerId = c.CreatedAssignerId,
                                       LastModifiedAssignerId = c.LastModifiedAssignerId,
                                   SubjectInfo = new SubjectSummaryModel
                                     {
                                         Id = s.Id,
                                         Name = s.Name
                                     }
                                   }) ;
            return classModelQuery.ToListAsync();
        }

        public Task<List<ClassViewModel>> GetAllClassInWeekByClassIdAndTeacherIdAsync(int semesterId, string teacherId)
        {
            var classModelQuery = (from se in _semesters
                                   join c in _classes on se.Id equals c.SemesterId
                                   join s in _dbSet on c.SubjectId equals s.Id
                                   where se.Id == semesterId && c.TeacherId == teacherId
                                   orderby c.ThuS, c.TietS, s.Name
                                   select new ClassViewModel
                                   {
                                       SemesterId = semesterId,
                                       ClassId = c.Id,
                                       ThuS = c.ThuS,
                                       TietS = c.TietS,
                                       MaLHP = c.MaLHP,
                                       LoaiHP = c.LoaiHP,
                                       Phong = c.Phong,
                                       TeacherId = c.TeacherId,
                                       TeacherEmail = c.TeacherEmail,
                                       CreatedAssignerId = c.CreatedAssignerId,
                                       LastModifiedAssignerId = c.LastModifiedAssignerId,
                                       SubjectInfo = new SubjectSummaryModel
                                       {
                                           Id = s.Id,
                                           Name = s.Name
                                       }
                                   });
            return classModelQuery.ToListAsync();
        }
        /// <summary>
        /// test lai
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Subject> AddAsync(Subject entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<Subject> GetBySubjectCodeAsync(string subjectCode)
        {
            if (string.IsNullOrEmpty(subjectCode))
                return null;
            var result = (from s in _dbSet
                         where s.SubjectCode == subjectCode
                         select s).FirstOrDefaultAsync();

            return result;
        }

        public Task<Subject> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            var result = (from s in _dbSet
                          where s.Name == name
                          select s).FirstOrDefaultAsync();

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subjectCode"></param>
        /// <returns></returns>
        public Task<Subject> GetByNameOrSubjectCodeAsync(string name, string subjectCode)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(subjectCode))
                return null;
            var result = (from s in _dbSet
                          where s.Name == name && s.SubjectCode == subjectCode
                          select s).FirstOrDefaultAsync();

            return result;
        }

        public Task<int> UpdateAsync()
        {
            return _context.SaveChangesAsync();
        }
        #endregion


    }
}
