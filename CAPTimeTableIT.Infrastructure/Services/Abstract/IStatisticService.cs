using CAPTimeTableIT.Infrastructure.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface IStatisticService
    {
        Task<List<StatisticForUser>> GetHoursBySemesterAsync(int semesterId);
        Task<List<StatisticForUser>> GetHoursByYearAsync(int startYear, int endYear);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<SummaryReport>> GetSummaryReportAsync();
        Task<List<SummaryReport>> GetYearlySummaryReportAsync();
    }
}
