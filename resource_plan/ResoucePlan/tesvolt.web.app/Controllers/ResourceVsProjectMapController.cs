using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using tesvolt.dbmodels.app;

namespace tesvolt.web.app.Controllers
{
    public class ResourceVsProjectMapController : Controller
    {
        private readonly TesvoltDbContext _context;

        public ResourceVsProjectMapController(TesvoltDbContext context)
        {
            _context = context;
        }

        // GET: ProjectVsResourceMap/Index
        public IActionResult Index()
        {
            // Return the view that contains the form and the result table
            return View();
        }

        // GET: ResourceVsProjectMap/ GetResourcesAndProjects
        [HttpGet]
        public async Task<IActionResult> GetResourcesAndProjects()
        {
            var resourcePlans = await _context.ResourceModels
    .GroupJoin(
        _context.ResourcePlanModels,
        r => r.Id,
        rp => rp.ResourceNameId,
        (r, resourcePlans) => new { r, resourcePlans }
    )
    .SelectMany(
        x => x.resourcePlans.DefaultIfEmpty(),
        (x, rp) => new
        {
            ResourceName = x.r.Name,
            ProjectName = rp != null ? rp.ProjectName.Name : " - ",
            Month = rp != null ? rp.Month : (int?)null,
            Year = rp != null ? rp.Year : (int?)null,
            Percentage = rp != null ? rp.Percentage : 0
        }
    )
    // Add ordering by ResourceName and ProjectName
    .OrderBy(x => x.ResourceName)         // Primary sort by ResourceName
    .ThenBy(x => x.ProjectName)           // Secondary sort by ProjectName
    .ToListAsync();
            return Json(resourcePlans);

        }
      
    }
    // In ResourceVsProjectMapController

}
