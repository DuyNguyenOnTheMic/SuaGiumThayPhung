using CAPTimeTableIT.Helpers;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CAPTimeTableIT.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        #region private fields
        private readonly IStatisticService _statisticService;
        private readonly ISemesterService _semesterService;
        #endregion

        public StatisticsController(IStatisticService statisticService, ISemesterService semesterService)
        {
            _statisticService = statisticService;
            _semesterService = semesterService;
        }

        // GET: Statistics for all semesters
        public async Task<ActionResult> Index(int id = 0)
        {
            var semesters = _semesterService.GetAll();
            var theLatestSemesterId = id;
            if (id == 0 && semesters.Count > 0)
            {
                theLatestSemesterId = semesters.Max(t => t.Id);
            }
            var report = await _statisticService.GetSummaryReportAsync();
            ViewBag.Semesters = new SelectList(_semesterService.GetAll(), "Id", "Name");
            ViewBag.LatestSemesterId = theLatestSemesterId;
            //validation
            return View(report);
        }

        // GET: Statistics for all semesters
        public async Task<ActionResult> YearlyReport()
        {
            var report = await _statisticService.GetYearlySummaryReportAsync();
            //validation
            return View(report);
        }

        /// <summary>
        /// base on semsters
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> StatisticsViewTab(int id = 0)
        {
            var semesters = _semesterService.GetAll();
            var theLatestSemesterId = id;
            if (id == 0)
            {
                theLatestSemesterId = semesters.Max(t => t.Id);
            }
            ViewBag.LatestSemesterId = theLatestSemesterId;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <returns></returns>
        public async Task<ActionResult> YearlyReportViewTab(int startYear, int endYear)
        {
            ViewBag.StartYear = startYear;
            ViewBag.Endyear = endYear;
            return View();
        }

        public async Task<ActionResult> TabularStatistics()
        {
            return View();
        }
        public async Task<ActionResult> Statistics(int semesterId = 0)
        {
            var semesters = _semesterService.GetAll();
            var theLatestSemesterId = semesterId;
            if (semesterId == 0)
            {
                theLatestSemesterId = semesters.Max(t => t.Id);
            }
            ViewBag.LatestSemesterId = theLatestSemesterId;
            return View();
        }

        /// <summary>
        /// Statistic by semesterId
        /// </summary>
        /// <param name="id">SemesterId</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetTeacherHourBySemesterId(int id)
        {
            var hours = await _statisticService.GetHoursBySemesterAsync(id);
            return new JsonCamelCaseResult(new
            {
                isSuccess = true,
                data = hours
            }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Statistic report by start year and end year
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetTeacherHourByStartYearAndEndYear(int startYear, int endYear)
        {
            var hours = await _statisticService.GetHoursByYearAsync(startYear, endYear);
            return new JsonCamelCaseResult(new
            {
                isSuccess = true,
                data = hours
            }, JsonRequestBehavior.DenyGet);

        }
    }
}