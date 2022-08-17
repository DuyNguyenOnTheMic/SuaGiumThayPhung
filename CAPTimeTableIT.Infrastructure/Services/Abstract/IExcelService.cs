using CAPTimeTableIT.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface IExcelService
    {
        Task<List<ErrorMessage>> ImportTimeTable(Stream stream, int semesterId);
    }
}
