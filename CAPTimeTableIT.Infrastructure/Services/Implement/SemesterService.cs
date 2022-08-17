using CAPTimeTableIT.Common;
using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Models;
using CAPTimeTableIT.Infrastructure.Models.Classes;
using CAPTimeTableIT.Infrastructure.Models.Semesters;
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
    public class SemesterService : ISemesterService
    {
        #region fields
        private readonly DbContext _context;
        private readonly DbSet<Semester> _dbSet;
        private readonly DbSet<Class> _classDbSet;
        private readonly IRepository<Semester> _semesterRepository;
        private const string SemesterPatternKey = "EF.Semester.";
        #endregion
        #region ctor
        public SemesterService(DbContext context, IRepository<Semester> semesterRepository)
        {
            _context = context;
            _dbSet = _context.Set<Semester>();
            _semesterRepository = semesterRepository;
            _classDbSet = _context.Set<Class>(); ;
        }
        #endregion
        #region methods
        public async Task<List<Semester>> GetAllAsync()
        {
            List<Semester> result;
            if (!CapstoneMemoryCache.Instance.Exists(SemesterPatternKey))
            {
                result = await _dbSet.AsQueryable().ToListAsync();
                CapstoneMemoryCache.Instance.Add<List<Semester>>(SemesterPatternKey, result, 20);
            }
            else
            {
                result = CapstoneMemoryCache.Instance.GetValue<List<Semester>>(SemesterPatternKey);
            }
            return result;
        }

        public List<Semester> GetAll()
        {
            List<Semester> result;
            if (!CapstoneMemoryCache.Instance.Exists(SemesterPatternKey))
            {
                result = _dbSet.AsQueryable().ToList();
                CapstoneMemoryCache.Instance.Add<List<Semester>>(SemesterPatternKey, result, 20);
            }
            else
            {
                result = CapstoneMemoryCache.Instance.GetValue<List<Semester>>(SemesterPatternKey);
            }
            return result;
        }

        public Task<List<SemesterSummary>> GetSemesterSummaries()
        {
            var result = _context.Database.SqlQuery<SemesterSummary>("[dbo].[usp_GetSummarySemester]").ToListAsync();
            return result;
        }

        public Task<Semester> GetByIdAsync(int id)
        {            
            var result = _dbSet.FirstOrDefaultAsync(t => t.Id == id);
            return result;
        }

        public Semester GetById(int id)
        {
            var result = _semesterRepository.Get(id);
            return result;
           /* return _dbSet.SqlQuery("select * from Semesters where id= @p0", id).FirstOrDefault();*/
        }

        public Task<Semester> AddAsync(Semester entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            var newsemester = _dbSet.Add(entity);
            _context.SaveChanges();
            CapstoneMemoryCache.Instance.Delete(SemesterPatternKey);
            return Task.FromResult(newsemester);
        }

        public async Task<int> UpdateAsync()
        {
            await _context.SaveChangesAsync();
            CapstoneMemoryCache.Instance.Delete(SemesterPatternKey);
            return 1;
        }

        public bool Delete(int id)
        {
            _semesterRepository.Delete(id);
            CapstoneMemoryCache.Instance.Delete(SemesterPatternKey);
            return true;
        }

        public Semester GetTheLatestSemester()
        {
            var result = (from s in _dbSet                         
                         orderby s.Id descending
                         select s).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Kiem tra hoc ky co ton tai 1 lop nao do hay ko
        /// </summary>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        public async Task<ValidationResult> ValidateDeleteSemesterIdAsync(int semesterId)
        {
            var result = new ValidationResult
            {
                Success = true,
                HasConfirm = false,
            };
            var semester = await GetByIdAsync(semesterId);
            if (semester == null)
            {
                result.Success = false;
                result.Message = "Semester is invalid";
                return result;
            }
            var hasExistedClass = _classDbSet.Any(t => t.SemesterId == semesterId);
            if (hasExistedClass)
            {
                result.Success = false;
                result.Message = "Da ton tai lop trong hoc ky nay";
            }
            return result;
        }

        public Semester FindSemester(string semesterName)
        {
            if (string.IsNullOrEmpty(semesterName)) return null;
            Semester result;
            if (!CapstoneMemoryCache.Instance.Exists(SemesterPatternKey))
            {
                result = _dbSet.FirstOrDefault(t => t.Name == semesterName);
            }
            else
            {
                result = CapstoneMemoryCache.Instance.GetValue<List<Semester>>(SemesterPatternKey).FirstOrDefault(t=>t.Name == semesterName);
            }
            return result;
        }

        public CountClassModel CountClass(int semesterId)
        {
            var classes = _classDbSet.Where(t => t.SemesterId == semesterId).Select(t=>new
            {
                Id = t.Id,
                TeacherId = t.TeacherId
            }).ToList();
            var result = new CountClassModel
            {
                TotalClass = classes.Count,
                TotalAssignedClass = classes.Where(t=>!string.IsNullOrEmpty(t.TeacherId)).Count()
            };
            return result;
        }
        #endregion
    }
}
