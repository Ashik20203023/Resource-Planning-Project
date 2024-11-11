using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using tesvolt.dbmodels.app;

namespace tesvolt.web.app.Controllers
{
    public class ProjectVsProjectRealisation : Controller
    {
        private readonly TesvoltDbContext _context;

        public ProjectVsProjectRealisation(TesvoltDbContext context)
        {
            _context = context;
        }

        // GET: ProjectVsProjectRealisation/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: ProjectVsProjectRealisation/GetAttendanceDetails
        [HttpGet]
        public async Task<IActionResult> GetAttendanceDetails(string startDate, string endDate)
        {
            // Parse the start and end dates
            if (!DateTime.TryParse(startDate, out DateTime start) ||
                !DateTime.TryParse(endDate, out DateTime end))
            {
                return BadRequest("Invalid date format.");
            }

            var attendanceDetails = await _context.AttendanceDetailEntryModels
                .Include(ad => ad.AttendanceEntry)
                .Include(ad => ad.ProjectName)
                .Include(ad => ad.ResourceName)
                .Where(ad => ad.AttendanceDate >= start && ad.AttendanceDate <= end)
                .Select(ad => new
                {
                    ProjectName = ad.ProjectName.Name,
                    ResourceName = ad.ResourceName.Name,
                    AttendanceDate = ad.AttendanceDate
                })
                .ToListAsync();

            var result = attendanceDetails
                .GroupBy(ad => new
                {
                    ad.ProjectName,
                    ad.ResourceName,
                    Year = ad.AttendanceDate.Year,
                    Month = ad.AttendanceDate.Month
                })
                .Select(g => new AttendanceDetailDto
                {
                    ProjectName = g.Key.ProjectName,
                    ResourceName = g.Key.ResourceName,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    AttendanceCount = g.Count(),
                    TotalDays = DateTime.DaysInMonth(g.Key.Year, g.Key.Month),
                    Percentage = (double)g.Count() / DateTime.DaysInMonth(g.Key.Year, g.Key.Month) * 100
                })
                .ToList();

            return Json(result);
        }
    }

    // DTO for Attendance Detail
    public class AttendanceDetailDto
    {
        public string ProjectName { get; set; }
        public string ResourceName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int AttendanceCount { get; set; }
        public int TotalDays { get; set; }
        public double Percentage { get; set; }
    }
}
