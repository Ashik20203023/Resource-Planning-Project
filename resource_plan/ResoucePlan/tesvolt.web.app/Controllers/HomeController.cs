using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using tesvolt.dbmodels.app;
using tesvolt.web.app.Models;

namespace tesvolt.web.app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TesvoltDbContext _context;

        public HomeController(ILogger<HomeController> logger, TesvoltDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Fetch counts
                var projectCount = await _context.ProjectModels.CountAsync();
                var resourceCount = await _context.ResourceModels.CountAsync();
                var currentDate = DateTime.Now;
                var currentMonth = currentDate.Month;
                var currentYear = currentDate.Year;

                var vacationCount = await _context.VacationPlanModels
                    .Where(v => v.StartDate.Month == currentMonth && v.StartDate.Year == currentYear &&
                                 v.EndDate.Month == currentMonth && v.EndDate.Year == currentYear)
                    .CountAsync();

                // Get project counts per resource
                var resourceProjectCounts = await _context.ResourcePlanModels
                    .Include(rp => rp.ResourceName) // Assuming ResourceName is a navigation property
                    .GroupBy(rp => rp.ResourceName.Name)
                    .Select(g => new ResourceProjectCount
                    {
                        ResourceName = g.Key,
                        ProjectCount = g.Count()
                    })
                    .ToListAsync();

                var viewModel = new HomeIndexViewModel
                {
                    ProjectCount = projectCount,
                    ResourceCount = resourceCount,
                    VacationCount = vacationCount,
                    ResourceProjectCounts = resourceProjectCounts // Add this line
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching data for the index page.");
                /* return View("Error");*/ // or return a more user-friendly error view
                throw ex;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Blank()
        {
            return View();
        }

        public IActionResult Form()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ResourceProjectCount
    {
        public string ResourceName { get; set; }
        public int ProjectCount { get; set; }
    }
}

public class HomeIndexViewModel
{
    internal List<tesvolt.web.app.Controllers.ResourceProjectCount> ResourceProjectCounts;

    public int ProjectCount { get; set; }
    public int ResourceCount { get; set; }
    public int VacationCount { get; set; } 
}