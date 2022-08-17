using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Models;
using CAPTimeTableIT.Infrastructure.Models.Subjects;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Data.SqlClient;
using CAPTimeTableIT.Common;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class ClassService : IClassService
    {
        #region constant
        private const string ClassPatternKey = "EF.class.";
        #endregion
        #region fields
        private readonly DbContext _context;
        private readonly DbSet<Class> _dbSet;
        private readonly DbSet<Semester> _semesterDbSet;
        private readonly DbSet<UserProfile> _userProfileDbSet;
        private readonly IUserService _userService;
        private readonly ISubjectService _subjectService;

        //private readonly IRepository<ApplicationRole> _applicationRepository;
        #endregion
        #region ctor
        public ClassService(DbContext context, IUserService userService, ISubjectService subjectService)
        {
            _context = context;
            _dbSet = _context.Set<Class>();
            _semesterDbSet = _context.Set<Semester>();
            _userProfileDbSet = _context.Set<UserProfile>();
            _userService = userService;
            _subjectService = subjectService;
        }
        #endregion
        #region methods
        public async Task<List<Class>> GetAllAsync()
        {
            List<Class> classes;
            if (!CapstoneMemoryCache.Instance.Exists(ClassPatternKey))
            {
                classes = await _dbSet.AsQueryable().ToListAsync();
                CapstoneMemoryCache.Instance.Add<List<Class>>(ClassPatternKey, classes, 20);
            }
            else
            {
                classes = CapstoneMemoryCache.Instance.GetValue<List<Class>>(ClassPatternKey);
            }
            
            return classes;
        }

        public List<Class> GetAll()
        {
            List<Class> classes;
            if (!CapstoneMemoryCache.Instance.Exists(ClassPatternKey))
            {
                classes = _dbSet.AsQueryable().ToList();
                CapstoneMemoryCache.Instance.Add<List<Class>>(ClassPatternKey, classes, 20);
            }
            else
            {
                classes = CapstoneMemoryCache.Instance.GetValue<List<Class>>(ClassPatternKey);
            }

            return classes;
        }

        public Task<Class> GetByIdAsync(int id)
        {
            return _dbSet.FirstOrDefaultAsync(t => t.Id == id);
        }

        public Class GetById(int id)
        {
            return _dbSet.FirstOrDefault(t => t.Id == id);
        }

        public Task<Class> AddAsync(Class entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            var newClass = _dbSet.Add(entity);
            _context.SaveChanges();
            CapstoneMemoryCache.Instance.Delete(ClassPatternKey);
            return Task.FromResult(newClass);
        }

        public Task<int> UpdateAsync(Class entity)
        {
            var result = _context.SaveChangesAsync();
            CapstoneMemoryCache.Instance.Delete(ClassPatternKey);
            return result;
        }

        public Task<int> DeleteAsync(Class entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Null");
            _dbSet.Remove(entity);
            var result = _context.SaveChangesAsync();
            CapstoneMemoryCache.Instance.Delete(ClassPatternKey);
            return result;
        }

        public async Task<int> DeleteAsync(int semesterId)
        {
            var query = string.Format("DELETE FROM Classes WHERE semesterId = {0}", semesterId);
            var countAffectedRow = _context.Database.ExecuteSqlCommand(query);
            CapstoneMemoryCache.Instance.Delete(ClassPatternKey);
            return countAffectedRow;
        }

        public Task<Class> ImportDataFromExcel(ApplicationDbContext db)
        {
            throw new NotImplementedException();
        }
        public Task<Class> AddCreatedAssignerIdAsync(string createdAssignerId)
        {
            //if (createdAssignerId == null)
            //    throw new ArgumentNullException("Null");
            //var createdAssignerID = _dbSet.Add(createdAssignerId);
            //_context.SaveChanges();
            //return Task.FromResult(createdAssignerID);
            throw new NotImplementedException();
        }


        public Task<Class> GetByMaLHPAsync(string maLHP)
        {
            if (string.IsNullOrEmpty(maLHP))
                return null;
            var result = (from s in _dbSet
                          where s.MaLHP == maLHP
                          select s).FirstOrDefaultAsync();

            return result;
        }

        public Task<Class> GetByThuSAsync(int thuS)
        {
            if (thuS == 0)
                return null;
            var result = (from s in _dbSet
                          where s.ThuS == thuS
                          select s).FirstOrDefaultAsync();

            return result;
        }

        public Task<Class> GetByTietSAsync(int tietS)
        {
            if (tietS == 0)
                return null;
            var result = (from s in _dbSet
                          where s.TietS == tietS
                          select s).FirstOrDefaultAsync();

            return result;
        }

        public Task<Class> GetByMaLHPAndThuSAsync(int semesterId, string maLHP, int thuS)
        {
            if (string.IsNullOrEmpty(maLHP) || thuS == 0 || semesterId == 0)
                return null;
            var result = (from c in _dbSet
                          join s in _semesterDbSet on c.SemesterId equals s.Id
                          where s.Id == semesterId && c.MaLHP == maLHP && c.ThuS == thuS
                          select c).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ValidationResult> ValidateTeacherInAClassAsync(string teacherId, int currentClassId)
        {
            var result = new ValidationResult
            {
                Success = true,
                HasConfirm = false,
            };
            var teacher = await _userService.GetByIdAsync(teacherId);
            var currentClass = await GetByIdAsync(currentClassId);
            if(currentClass == null)
            {
                result.Success = false;
                result.Message = "Class is invalid";
                return result;
            }
            if (currentClass.TeacherId == teacherId) return result;
            //check duplicate teacher
            var captoneClass = await GetByTeacherIdAndThuSAsync(currentClass.SemesterId.Value, currentClass.ThuS.Value, currentClass.TietS.Value, teacherId);
            if (captoneClass != null)
            {
                var subject = await _subjectService.GetByIdAsync(captoneClass.SubjectId.Value);
                result.Success = false;
                result.Message = $"{teacher.UserName} đã dạy lớp {subject.Name}";
                return result;
            }
            //------------Check condition 2: check 3 classes-------------
            var query = $"SELECT count(1)  FROM [dbo].[Classes] where semesterId = {currentClass.SemesterId.Value} and thuS = {currentClass.ThuS.Value} and teacherId = '{teacherId}'";
            var countResult = await _context.Database.SqlQuery<int>(query).FirstOrDefaultAsync();
            if(countResult + 1 > 3)
            {
                result.Success = false;
                result.Message = $"{teacher.UserName} đã dạy quá 3 lớp. Bạn có muốn tiếp tục lưu không?";
                result.HasConfirm = true;
                return result;
            }
            //------------Check condition 3-------------
            var query2 = $"SELECT count(1)  FROM [dbo].[Classes] where semesterId = {currentClass.SemesterId.Value} and teacherId = '{teacherId}'";
            var countResult2 = await _context.Database.SqlQuery<int>(query2).FirstOrDefaultAsync();
            if (countResult2 + 1 > 8)
            {
                result.Success = false;
                result.Message = $"{teacher.UserName} đã dạy quá 8 lớp trong 1 học kỳ";
                result.HasConfirm = true;
                return result;
            }
            return result;
        }

        public Task<Class> GetByTeacherIdAndThuSAsync(int semesterId, int thuS, int tietS, string teacherId)
        {
            var result = (from c in _dbSet                          
                          where c.SemesterId == semesterId && c.ThuS == thuS && c.TietS == tietS && c.TeacherId == teacherId
                          select c).FirstOrDefaultAsync();
            return result;
        }

        public async Task<byte[]> ExportTimeTableAsync(int semesterId)
        {
            var headers = new List<string>
            {
                "MaGocLHP", "Mã MH", "Mã LHP", "Tên HP", "Số TC", "Loại HP", "Mã Lớp", "TSMH", "Số Tiết Đã xếp", "PH", "Thứ",
                "Tiết BĐ", "Số Tiết", "Tiết Học", "Phòng", "Mã CBGD", "Tên CBGD", "PH_X", "Sức Chứa", "SiSoTKB", "Trống", "Tình Trạng LHP", "TuanHoc2",
                "ThuS", "TietS", "Số SVĐK", "Tuần BD", "Tuần KT", "Mã Ngành", "Tên Ngành", "Ghi Chú"
            };
            var query = string.Format(@"SELECT c.[Id],c.[SemesterId]
                                          ,c.[SubjectId], s.SubjectCode, s.Name
                                          ,c.[CreatedDateTime]
                                          ,c.[LastModifiedDateTime]
                                          ,c.[PH]
                                          ,c.[PH_X]
                                          ,c.[SoTietDaXep]
                                          ,c.[ThuS]
                                          ,c.[TietS]
                                          ,c.[TMSH]
                                          ,c.[MaGocLHP]
                                          ,c.[MaLHP]
                                          ,c.[SoTC]
                                          ,c.[LoaiHP]
                                          ,c.[MaLop]
                                          ,c.[Thu]
                                          ,c.[TietBD]
                                          ,c.[SoTiet]
                                          ,c.[TietHoc]
                                          ,c.[Phong]
                                          ,c.[SucChua]
                                          ,c.[SiSoTKB]
                                          ,c.[Trong]
                                          ,c.[TinhTrangLHP]
                                          ,c.[TuanHoc2]
                                          ,c.[SoSVDK]
                                          ,c.[TuanBD]
                                          ,c.[TuanKT]
                                          ,c.[MaNganh]
                                          ,c.[TenNganh]
                                          ,c.[Note]
                                          ,c.[TeacherId]
                                    , up.FullName, up.StaffCode FROM Classes c 
                            INNER JOIN Subjects s ON c.SubjectId = s.Id
                            LEFT JOIN UserProfiles up ON c.TeacherId =  up.Id WHERE c.SemesterId = {0}", semesterId);
            var exportData = await _context.Database.SqlQuery<ExportClassModel>(query).ToListAsync();

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var memoryStream = new MemoryStream();
            using (var package = new ExcelPackage())
            {
                //Add a new worksheet to the empty workbook
                var worksheet = package.Workbook.Worksheets.Add("Semester");
                //Add the headers
                #region add headers
                var colorValue = ColorTranslator.FromHtml("#FFE699");
                var font = new Font("Calibri", 11);
                for (var i = 0; i < headers.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.SetFromFont(font);
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.SetBackground(colorValue);
                    worksheet.Cells[1, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                #endregion

                #region add body data
                var rowIndex = 2;
                for(var i = 0; i < exportData.Count; i++)
                {
                    worksheet.Cells[rowIndex, 1].Value = exportData[i].MaGocLHP;
                    worksheet.Cells[rowIndex, 2].Value = exportData[i].SubjectCode;
                    worksheet.Cells[rowIndex, 3].Value = exportData[i].MaGocLHP;
                    worksheet.Cells[rowIndex, 4].Value = exportData[i].Name;
                    worksheet.Cells[rowIndex, 5].Value = exportData[i].SoTC;
                    worksheet.Cells[rowIndex, 6].Value = exportData[i].LoaiHP;
                    worksheet.Cells[rowIndex, 7].Value = exportData[i].MaLop;
                    worksheet.Cells[rowIndex, 8].Value = exportData[i].TMSH;
                    worksheet.Cells[rowIndex, 9].Value = exportData[i].SoTiet;
                    //worksheet.Cells[rowIndex, 10].Value = exportData[i].MaGocLHP;
                    worksheet.Cells[rowIndex, 11].Value = exportData[i].Thu;
                    worksheet.Cells[rowIndex, 12].Value = exportData[i].TietBD;
                    worksheet.Cells[rowIndex, 13].Value = exportData[i].SoTiet;
                    worksheet.Cells[rowIndex, 14].Value = exportData[i].TietHoc;
                    worksheet.Cells[rowIndex, 15].Value = exportData[i].Phong;
                    worksheet.Cells[rowIndex, 16].Value = exportData[i].StaffCode;//Mã CBGD
                    worksheet.Cells[rowIndex, 17].Value = exportData[i].FullName;
                    worksheet.Cells[rowIndex, 18].Value = exportData[i].PH_X;
                    worksheet.Cells[rowIndex, 19].Value = exportData[i].SucChua;
                    worksheet.Cells[rowIndex, 20].Value = exportData[i].SiSoTKB;
                    worksheet.Cells[rowIndex, 21].Value = exportData[i].Trong;
                    worksheet.Cells[rowIndex, 22].Value = exportData[i].TinhTrangLHP;
                    worksheet.Cells[rowIndex, 23].Value = exportData[i].TuanHoc2;
                    worksheet.Cells[rowIndex, 24].Value = exportData[i].ThuS;
                    worksheet.Cells[rowIndex, 25].Value = exportData[i].TietS;
                    worksheet.Cells[rowIndex, 26].Value = exportData[i].SoSVDK;
                    worksheet.Cells[rowIndex, 27].Value = exportData[i].TuanBD;
                    worksheet.Cells[rowIndex, 28].Value = exportData[i].TuanKT;
                    worksheet.Cells[rowIndex, 29].Value = exportData[i].MaNganh;
                    worksheet.Cells[rowIndex, 30].Value = exportData[i].TenNganh;
                    worksheet.Cells[rowIndex, 31].Value = exportData[i].Note;
                    rowIndex++;
                }
                var allCells = worksheet.Cells[2, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.SetFromFont(font);
                //--Add border for row 2 to row n
                for(var i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    for (var j = 1; j <= worksheet.Dimension.End.Column; j++)
                    {
                        worksheet.Cells[i, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[i, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[i, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }
                #endregion
                await package.SaveAsAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ToArray();
        }

        public int AddRange(IList<Class> entity)
        {
            if(entity == null || entity.Count == 0) return 0;
            var classModel = new DataTable();
            classModel.Columns.Add("SemesterId", typeof(int));
            classModel.Columns.Add("MaGocLHP", typeof(string));
            classModel.Columns.Add("MaLHP", typeof(string));
            classModel.Columns.Add("SoTC", typeof(int));
            classModel.Columns.Add("LoaiHP", typeof(string));
            classModel.Columns.Add("MaLop", typeof(string));
            classModel.Columns.Add("TMSH", typeof(int));
            classModel.Columns.Add("SoTietDaXep", typeof(int));
            classModel.Columns.Add("PH", typeof(string));
            classModel.Columns.Add("Thu", typeof(string));
            classModel.Columns.Add("TietBD", typeof(int));
            classModel.Columns.Add("SoTiet", typeof(int));
            classModel.Columns.Add("TietHoc", typeof(string));
            classModel.Columns.Add("Phong", typeof(string));
            classModel.Columns.Add("PH_X", typeof(string));
            classModel.Columns.Add("SucChua", typeof(int));
            classModel.Columns.Add("SiSoTKB", typeof(int));
            classModel.Columns.Add("Trong", typeof(int));
            classModel.Columns.Add("TinhTrangLHP", typeof(string));
            classModel.Columns.Add("TuanHoc2", typeof(string));
            classModel.Columns.Add("ThuS", typeof(int));
            classModel.Columns.Add("TietS", typeof(int));
            classModel.Columns.Add("SoSVDK", typeof(int));
            classModel.Columns.Add("TuanBD", typeof(int));
            classModel.Columns.Add("TuanKT", typeof(int));
            classModel.Columns.Add("MaNganh", typeof(string));
            classModel.Columns.Add("TenNganh", typeof(string));
            classModel.Columns.Add("Note", typeof(string));
            classModel.Columns.Add("SubjectId", typeof(int));
            foreach(var item in entity)
            {
                classModel.Rows.Add(new object[] { item.SemesterId, item.MaGocLHP, item.MaLHP, item.SoTC, item.LoaiHP,
                    item.MaLop, item.TMSH, item.SoTietDaXep, item.PH, item.Thu,
                    item.TietBD, item.SoTiet, item.TietHoc, item.Phong, item.PH_X,
                    item.SucChua, item.SiSoTKB, item.Trong, item.TinhTrangLHP, item.TuanHoc2,
                    item.ThuS, item.TietS, item.SoSVDK, item.TuanBD, item.TuanKT,
                    item.MaNganh, item.TenNganh, item.Note, item.SubjectId
                });
            }
            var dataTableParam = new SqlParameter($"@classes", classModel)
            {
                TypeName = "dbo.ClassInsertedModel"
            };
            return _context.Database.SqlQuery<int>("[dbo].[usp_Class_Insert] @classes",
                dataTableParam).FirstOrDefault();
        }

        public List<Class> GetBySemesterId(int semesterId)
        {
            return _dbSet.Where(x => x.SemesterId == semesterId).ToList();
        }
        #endregion
    }
}
        
