using CAPTimeTableIT.Infrastructure.Models.Statistics;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class StatisticService : IStatisticService
    {
        private readonly DbContext _context;
        public StatisticService(DbContext context)
        {
            _context = context;
        }
        public Task<List<StatisticForUser>> GetHoursBySemesterAsync(int semesterId)
        {
            var query = $@"SELECT B.Id UserId, IsNull(B.FullName, au.Email) As FullName, A.TotalHours FROM
                    (SELECT SUM(SoTietDaXep) AS TotalHours, TeacherId FROM classes
                    WHERE semesterId = {semesterId} AND teacherId IS NOT NULL
                    GROUP BY teacherId) AS A
                    INNER JOIN AspNetUsers au ON A.TeacherId = au.Id
		            LEFT JOIN UserProfiles B ON au.Id = B.Id";
            var result = _context.Database.SqlQuery<StatisticForUser>(query).ToListAsync();
            return result;
        }
        public Task<List<StatisticForUser>> GetHoursByYearAsync(int startYear, int endYear)
        {
            var startYearParam = new SqlParameter("@startYear", startYear);
            var endYearParam = new SqlParameter("@endYear", endYear);
            var result = _context.Database.SqlQuery<StatisticForUser>("[dbo].[usp_GetDetailReportByYear] @startYear, @endYear", startYearParam, endYearParam).ToListAsync();
            return result;
        }

        public Task<List<SummaryReport>> GetSummaryReportAsync()
        {
            var result = _context.Database.SqlQuery<SummaryReport>("[dbo].[usp_GetSemesterSummaryReport]").ToListAsync();
            return result;
        }

        public Task<List<SummaryReport>> GetYearlySummaryReportAsync()
        {
            var result = _context.Database.SqlQuery<SummaryReport>("[dbo].[usp_GetYearlySummaryReport]").ToListAsync();
            return result;
        }

    }
}