using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using tesvolt.dbmodels.app;

namespace tesvolt.web.app.Controllers
{
    public class ProjectVsResourceMapController : Controller
    {
        private readonly TesvoltDbContext _context;

        public ProjectVsResourceMapController(TesvoltDbContext context)
        {
            _context = context;
        }

        // GET: ProjectVsResourceMap/Index
        public IActionResult Index()
        {
            // Return the view that contains the form and the result table
            return View();
        }

        // GET: ProjectVsResourceMap/GetProjectsAndResources
        [HttpGet]
        public async Task<IActionResult> GetProjectsAndResources()
        {
            var resourcePlans = await _context.ResourceModels
                .GroupJoin(
                    _context.ResourcePlanModels,
                    p => p.Id,                      // Project ID from ProjectModel
                    rp => rp.ResourceNameId,         // Assuming ResourcePlanModel has a ProjectNameId property
                    (p, resourcePlans) => new { p, resourcePlans }
                )
                .SelectMany(
                    x => x.resourcePlans.DefaultIfEmpty(), // Left join
                    (x, rp) => new
                    {
                        ResourceName = x.p.Name,                 // Get project name
                       ProjectName = rp != null ? rp.ProjectName.Name : "-", // Handle nulls
                        Month = rp != null ? rp.Month : (int?)null, // Handle nulls
                        Year = rp != null ? rp.Year : (int?)null,   // Handle nulls
                        Percentage = rp != null ? rp.Percentage : 0  // Handle nulls
                    }
                )
                .ToListAsync();

            return Json(resourcePlans);
        }
    }
}
