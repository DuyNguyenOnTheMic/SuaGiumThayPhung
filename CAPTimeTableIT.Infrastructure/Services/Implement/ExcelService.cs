using CAPTimeTableIT.Common;
using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Models;
using CAPTimeTableIT.Infrastructure.Models.Classes;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class ExcelService: IExcelService
    {
        private readonly ISubjectService _subjectService;
        private readonly IClassService _classService;
        public ExcelService(ISubjectService subjectService, IClassService classService)
        {
            _subjectService = subjectService;
            _classService = classService;
        }

        public async Task<List<ErrorMessage>> ImportTimeTable(Stream stream, int semesterId)
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var messageErrors = new List<ErrorMessage>();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                //int colCount = worksheet.Dimension.End.Column;  //get Column Count
                var rowCount = worksheet.Dimension.End.Row;     //get row count
                IList<Class> newClasses = new List<Class>();
                var memoryClasses = new List<KeyClass>();
                //collect all classes in files
                var classInDb = _classService.GetBySemesterId(semesterId);
                var listRowIndex = new List<int>();
                for (var i = 2; i <= rowCount; i++)
                {
                    var maLHP = worksheet.Cells[i, 3].Value.ToString();
                    var thuS = int.Parse(worksheet.Cells[i, 24].Value.ToString());
                    var tietS = int.Parse(worksheet.Cells[i, 25].Value.ToString());
                    var thu = worksheet.Cells[i, 11].Value.ToStringOrEmpty();
                    var validation = ValidateTimeTableRow(i, maLHP, thuS, tietS, memoryClasses);
                    var tietBD = int.Parse(worksheet.Cells[i, 12].Value.ToStringOrEmpty());
                    if (validation != null && validation.Count > 0)
                    {
                        messageErrors.AddRange(validation);
                        continue;
                    }
                    memoryClasses.Add(new KeyClass
                    {
                        RowIndex = i,
                        MaLHP = maLHP,
                        ThuS = thuS,
                        TietS = tietS,
                        Thu = thu,
                        TietBd = tietBD
                    });
                    listRowIndex.Add(i);
                }
                try
                {
                    var maLhpList = memoryClasses.Select(t => t.MaLHP).Distinct().ToList();
                    foreach (var maLhp in maLhpList)
                    {
                        var maLhpDb = classInDb.Where(t => t.MaLHP == maLhp).ToList();
                        if (maLhpDb.Count == 0)
                        {
                            //add all new classes
                            #region new classes
                            foreach (var memoryClass in memoryClasses.Where(t => t.MaLHP == maLhp))
                            {
                                var i = memoryClass.RowIndex;
                                var thuS = int.Parse(worksheet.Cells[i, 24].Value.ToString());
                                var tietS = int.Parse(worksheet.Cells[i, 25].Value.ToString());
                                var subjectCode = worksheet.Cells[i, 2].Value.ToString();
                                var subjectName = worksheet.Cells[i, 4].Value.ToString();
                                var tblSubject = await _subjectService.GetByNameOrSubjectCodeAsync(subjectName, subjectCode);

                                if (tblSubject == null)
                                {
                                    tblSubject = new Subject
                                    {
                                        SubjectCode = subjectCode,
                                        Name = subjectName,
                                    };
                                    await _subjectService.AddAsync(tblSubject);
                                }
                                var tblClass = new Class()
                                {
                                    SemesterId = semesterId,
                                    MaGocLHP = worksheet.Cells[i, 1].Value.ToStringOrEmpty(),
                                    MaLHP = worksheet.Cells[i, 3].Value.ToStringOrEmpty(),
                                    SoTC = int.Parse(worksheet.Cells[i, 5].Value.ToStringOrEmpty()),//Convert.ToInt32(worksheet.Cells[i,"Số TC"]),
                                    LoaiHP = worksheet.Cells[i, 6].Value.ToStringOrEmpty(),
                                    MaLop = worksheet.Cells[i, 7].Value.ToStringOrEmpty(),//worksheet.Cells[i,2,3]
                                    TMSH = int.Parse(worksheet.Cells[i, 8].Value.ToStringOrEmpty()),
                                    SoTietDaXep = int.Parse(worksheet.Cells[i, 9].Value.ToStringOrEmpty()),
                                    PH = worksheet.Cells[i, 10].Value.ToStringOrEmpty(),
                                    Thu = worksheet.Cells[i, 11].Value.ToStringOrEmpty(),
                                    TietBD = int.Parse(worksheet.Cells[i, 12].Value.ToStringOrEmpty()),
                                    SoTiet = int.Parse(worksheet.Cells[i, 13].Value.ToStringOrEmpty()),
                                    TietHoc = worksheet.Cells[i, 14].Value.ToStringOrEmpty(),
                                    Phong = worksheet.Cells[i, 15].Value.ToStringOrEmpty(),
                                    PH_X = worksheet.Cells[i, 18].Value.ToStringOrEmpty(),
                                    SucChua = int.Parse(worksheet.Cells[i, 19].Value.ToStringOrEmpty()),
                                    SiSoTKB = int.Parse(worksheet.Cells[i, 20].Value.ToStringOrEmpty()),
                                    Trong = int.Parse(worksheet.Cells[i, 21].Value.ToStringOrEmpty()),
                                    TinhTrangLHP = worksheet.Cells[i, 22].Value.ToStringOrEmpty(),
                                    TuanHoc2 = worksheet.Cells[i, 23].Value.ToStringOrEmpty(),
                                    ThuS = thuS,
                                    TietS = tietS,
                                    SoSVDK = int.Parse(worksheet.Cells[i, 26].Value.ToStringOrEmpty()),
                                    TuanBD = int.Parse(worksheet.Cells[i, 27].Value.ToStringOrEmpty()),
                                    TuanKT = int.Parse(worksheet.Cells[i, 28].Value.ToStringOrEmpty()),
                                    MaNganh = worksheet.Cells[i, 29].Value.ToStringOrEmpty(),
                                    TenNganh = worksheet.Cells[i, 30].Value.ToStringOrEmpty(),
                                    Note = worksheet.Cells[i, 31].Value.ToStringOrEmpty(),
                                    SubjectId = tblSubject.Id,
                                };
                                newClasses.Add(tblClass);
                            }
                            #endregion
                        }
                        else
                        {
                            //if (maLhpDb.Count == 1)
                            var maLhpInMemory = memoryClasses.Where(t => t.MaLHP == maLhp).ToList();
                            if (maLhpDb.Count == 1 && maLhpInMemory.Count == 1)
                            {
                                maLhpDb[0].ThuS = maLhpInMemory[0].ThuS;
                                maLhpDb[0].TietS = maLhpInMemory[0].TietS;
                                maLhpDb[0].Thu = maLhpInMemory[0].Thu;
                                maLhpDb[0].TietBD = maLhpInMemory[0].TietBd;
                                await _classService.UpdateAsync(maLhpDb[0]);
                            }
                            else
                            {
                                //get classes that has same ThuS in DB and in file
                                var query = (from a in maLhpDb
                                             join b in maLhpInMemory
                                             on a.ThuS equals b.ThuS
                                             select a).ToList();
                                foreach (var item in maLhpDb)
                                {
                                    //remove class in db
                                    var hasClass = query.Any(t => t.ThuS == item.ThuS);
                                    if (!hasClass)
                                    {
                                        await _classService.DeleteAsync(item);
                                        continue;
                                    }
                                    //case class that exits in DB and memory
                                    
                                    //update
                                    var itemInFile = maLhpInMemory.FirstOrDefault(t => t.ThuS == item.ThuS);
                                    if (itemInFile != null && itemInFile.TietS != item.TietS)
                                    {
                                        item.TietS = itemInFile.TietS;
                                        item.TietBD = itemInFile.TietBd;
                                        await _classService.UpdateAsync(item);
                                    }
                                }
                                var newClassesInMemory = maLhpInMemory.Where(t => !maLhpDb.Any(m => m.ThuS == t.ThuS));
                                foreach (var item in newClassesInMemory)
                                {
                                    #region new classes
                                    var memoryClass = memoryClasses.FirstOrDefault(t => t.MaLHP == maLhp && t.ThuS == item.ThuS && t.TietS == item.TietS);
                                    if(memoryClass != null)
                                    //foreach (var memoryClass in memoryClasses.Where(t => t.MaLHP == maLhp && t.ThuS == item.ThuS && t.TietS == item.TietS))
                                    {
                                        var i = memoryClass.RowIndex;
                                        var thuS = int.Parse(worksheet.Cells[i, 24].Value.ToString());
                                        var tietS = int.Parse(worksheet.Cells[i, 25].Value.ToString());
                                        var subjectCode = worksheet.Cells[i, 2].Value.ToString();
                                        var subjectName = worksheet.Cells[i, 4].Value.ToString();
                                        var tblSubject = await _subjectService.GetByNameOrSubjectCodeAsync(subjectName, subjectCode);

                                        if (tblSubject == null)
                                        {
                                            tblSubject = new Subject
                                            {
                                                SubjectCode = subjectCode,
                                                Name = subjectName,
                                            };
                                            await _subjectService.AddAsync(tblSubject);
                                        }
                                        var tblClass = new Class()
                                        {
                                            SemesterId = semesterId,
                                            MaGocLHP = worksheet.Cells[i, 1].Value.ToStringOrEmpty(),
                                            MaLHP = worksheet.Cells[i, 3].Value.ToStringOrEmpty(),
                                            SoTC = int.Parse(worksheet.Cells[i, 5].Value.ToStringOrEmpty()),//Convert.ToInt32(worksheet.Cells[i,"Số TC"]),
                                            LoaiHP = worksheet.Cells[i, 6].Value.ToStringOrEmpty(),
                                            MaLop = worksheet.Cells[i, 7].Value.ToStringOrEmpty(),//worksheet.Cells[i,2,3]
                                            TMSH = int.Parse(worksheet.Cells[i, 8].Value.ToStringOrEmpty()),
                                            SoTietDaXep = int.Parse(worksheet.Cells[i, 9].Value.ToStringOrEmpty()),
                                            PH = worksheet.Cells[i, 10].Value.ToStringOrEmpty(),
                                            Thu = worksheet.Cells[i, 11].Value.ToStringOrEmpty(),
                                            TietBD = int.Parse(worksheet.Cells[i, 12].Value.ToStringOrEmpty()),
                                            SoTiet = int.Parse(worksheet.Cells[i, 13].Value.ToStringOrEmpty()),
                                            TietHoc = worksheet.Cells[i, 14].Value.ToStringOrEmpty(),
                                            Phong = worksheet.Cells[i, 15].Value.ToStringOrEmpty(),
                                            PH_X = worksheet.Cells[i, 18].Value.ToStringOrEmpty(),
                                            SucChua = int.Parse(worksheet.Cells[i, 19].Value.ToStringOrEmpty()),
                                            SiSoTKB = int.Parse(worksheet.Cells[i, 20].Value.ToStringOrEmpty()),
                                            Trong = int.Parse(worksheet.Cells[i, 21].Value.ToStringOrEmpty()),
                                            TinhTrangLHP = worksheet.Cells[i, 22].Value.ToStringOrEmpty(),
                                            TuanHoc2 = worksheet.Cells[i, 23].Value.ToStringOrEmpty(),
                                            ThuS = thuS,
                                            TietS = tietS,
                                            SoSVDK = int.Parse(worksheet.Cells[i, 26].Value.ToStringOrEmpty()),
                                            TuanBD = int.Parse(worksheet.Cells[i, 27].Value.ToStringOrEmpty()),
                                            TuanKT = int.Parse(worksheet.Cells[i, 28].Value.ToStringOrEmpty()),
                                            MaNganh = worksheet.Cells[i, 29].Value.ToStringOrEmpty(),
                                            TenNganh = worksheet.Cells[i, 30].Value.ToStringOrEmpty(),
                                            Note = worksheet.Cells[i, 31].Value.ToStringOrEmpty(),
                                            SubjectId = tblSubject.Id,
                                        };
                                        newClasses.Add(tblClass);
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    var totalRowUpdated = _classService.AddRange(newClasses);
                }
                catch (Exception e)
                {
                    //todo write log
                }


            }
            return messageErrors;
        }


        private List<ErrorMessage> ValidateTimeTableRow(int rowIndex, string maLHP, int thuS, int tietS, IList<KeyClass> newClasses)
        {
            var result = new List<ErrorMessage>();
            //check existed class in temporary list
            var existingClass = newClasses.FirstOrDefault(t=>t.MaLHP == maLHP && t.ThuS == thuS && t.TietS == tietS);
            if(existingClass != null)
            {
                result.Add(new ErrorMessage
                {
                    RowIndex = rowIndex,
                    PropertyName = "Ma Goc LHP",
                    Value = tietS.ToString(),
                    Message = "Ma Goc LHP, thu S, tietS bị trùng",
                });
                return result;
            }
            
            //int.TryParse(tietS, out int tietSResult);
            if (tietS == 0)
            {
                result.Add(new ErrorMessage
                {
                    RowIndex = rowIndex,
                    PropertyName = "Tiết S",
                    Value = tietS.ToString(),
                    Message = "dữ liệu không hợp lệ",
                });
                return result;
            }
            var validTietS = new List<int> { 1, 4, 7, 10, 13 };
            if (!validTietS.Contains(tietS))
            {
                result.Add(new ErrorMessage
                {
                    RowIndex = rowIndex,
                    PropertyName = "Tiết S",
                    Value = tietS.ToString(),
                    Message = " tiết học không hợp lệ",
                });
            }
            
            return result;
        }
    }


}
