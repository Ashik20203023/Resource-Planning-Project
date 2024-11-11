using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tesvolt.dbmodels.app;
using tesvolt.web.app.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;
namespace tesvolt.web.app.Controllers
{
    public class ResourcePlanController : Controller
    {
        private readonly TesvoltDbContext _context;

        public ResourcePlanController(TesvoltDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVacationData(int resourceNameId)
        {
            var vacationPlans = await _context.VacationPlanModels
                .Where(v => v.ResourceModelId == resourceNameId)
                .ToListAsync();

            var vacationData = vacationPlans.Select(v => new
            {
                vacationStart = v.StartDate.ToString("yyyy-MM-dd"),
                vacationEnd = v.EndDate.ToString("yyyy-MM-dd"),
                month = v.StartDate.ToString("MMMM")
            }).ToList();

            var vacationDays = vacationPlans.Sum(v => (v.EndDate - v.StartDate).Days + 1);

            return Json(new { data = vacationData });
        }

        [HttpGet]
        public async Task<IActionResult> GetModuleData(int moduleNameId)
        {
            var moduleDetails = await _context.ModuleModels
                .Where(v => v.Id == moduleNameId)
                .FirstOrDefaultAsync();

            return Json(new { data = moduleDetails });
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectData(string months, int year, int resourceNameId)
        {
            var monthArray = months.Split(',').Select(int.Parse).ToArray();

            var resourcePlans = await _context.ResourcePlanModels
                .Where(v => monthArray.Contains(v.Month) && v.Year == year && v.ResourceNameId == resourceNameId)
                .Include(x => x.ResourceName)
                .Include(x => x.ProjectName)
                .ToListAsync();

            return Json(new { data = resourcePlans });
        }



        // GET: ResourcePlan
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; // Number of records per page
            int pageNumber = page ?? 1; // Default to page 1 if not specified
            var resourcePlans = _context.ResourcePlanModels
              .Include(r => r.ProjectName)
              .Include(r => r.ResourceName)
              .OrderBy(r => r.ResourceName.Name)
              .ToPagedList(pageNumber, pageSize);


            return View(resourcePlans);
        }
        // GET: ResourcePlan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourcePlanModel = await _context.ResourcePlanModels
                .Include(r => r.ProjectName)
                .Include(r => r.ResourceName)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (resourcePlanModel == null)
            {
                return NotFound();
            }

            return View(resourcePlanModel);
        }

        // GET: ResourcePlan/Create
        public IActionResult Create()
        {
            ViewData["ProjectNameId"] = new SelectList(_context.ProjectModels, "Id", "Name");
            ViewData["ResourceNameId"] = new SelectList(_context.ResourceModels, "Id", "Name");
            ViewData["ModuleNameId"] = new SelectList(_context.ModuleModels, "Id", "Modules");
            string host = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            ViewData["BaseUrl"] = host;
            
            return View();
        }

        // POST: ResourcePlan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceNameId,ProjectNameId,ModuleNameId")] ResourcePlanModelForFormSubmit resourcePlanModel)
        {
            if (resourcePlanModel.ModuleNameId.HasValue && resourcePlanModel.ProjectNameId.HasValue && resourcePlanModel.ResourceNameId.HasValue)
            {
                // Create a new ResourcePlan entry for each selected month
                var newResourcePlan = new ResourcePlanModel
                {
                    ResourceNameId = resourcePlanModel.ResourceNameId,
                    //Month = month,
                    //Year = resourcePlanModel.Year,
                    ProjectNameId = resourcePlanModel.ProjectNameId,
                    //Percentage = resourcePlanModel.Percentage
                };
                _context.Add(newResourcePlan);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //If no months are selected, reload the view with validation error
            ViewData["ProjectNameId"] = new SelectList(_context.ProjectModels, "Id", "Name", resourcePlanModel.ProjectNameId);
            ViewData["ResourceNameId"] = new SelectList(_context.ResourceModels, "Id", "Name", resourcePlanModel.ResourceNameId);
            ViewData["ModulesNameId"] = new SelectList(_context.ModuleModels, "Id", "Name");
            //ViewBag.ResourceNameId = new SelectList(_context.Resources, "Id", "Name");
            //ViewBag.ProjectNameId = new SelectList(_context.Projects, "Id", "Name");
            //ViewBag.ModulesModal = new SelectList(_context.Modules, "Id", "Name"); // Adjust "Name" if necessary
            return View(resourcePlanModel);
        }


        // GET: ResourcePlan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourcePlanModel = await _context.ResourcePlanModels
                .FirstOrDefaultAsync(m => m.Id == id);

            if (resourcePlanModel == null)
            {
                return NotFound();
            }

            // Populate dropdown lists to keep them consistent (although not editable)
            ViewData["ProjectNameId"] = new SelectList(await _context.ProjectModels.ToListAsync(), "Id", "Name", resourcePlanModel.ProjectNameId);
            ViewData["ResourceNameId"] = new SelectList(await _context.ResourceModels.ToListAsync(), "Id", "Name", resourcePlanModel.ResourceNameId);
            ViewData["ModulesNameId"] = new SelectList(_context.ModuleModels, "Id", "Name");

            // Return the model with only the Percentage editable
            return View(resourcePlanModel);
        }

        // POST: ResourcePlan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceNameId,ProjectNameId,ModuleNameId")] ResourcePlanModel resourcePlanModel)
        {
            if (id != resourcePlanModel.Id)
            {
                return NotFound();
            }

            // Retrieve the existing model
            var existingModel = await _context.ResourcePlanModels.FindAsync(id);
            if (existingModel == null)
            {
                return NotFound();
            }

            // Update only the Percentage field
            existingModel.Percentage = resourcePlanModel.Percentage;

            //  if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existingModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourcePlanModelExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown lists in case of an error
            ViewData["ProjectNameId"] = new SelectList(await _context.ProjectModels.ToListAsync(), "Id", "Name", existingModel.ProjectNameId);
            ViewData["ResourceNameId"] = new SelectList(await _context.ResourceModels.ToListAsync(), "Id", "Name", existingModel.ResourceNameId);
            ViewData["ModulesNameId"] = new SelectList(_context.ModuleModels, "Id", "Name");

            return View(existingModel);
        }

        // GET: ResourcePlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourcePlanModel = await _context.ResourcePlanModels
                .Include(r => r.ProjectName)
                .Include(r => r.ResourceName)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (resourcePlanModel == null)
            {
                return NotFound();
            }

            return View(resourcePlanModel);
        }

        // POST: ResourcePlan/Delete/5
        // POST: Resource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resourcePlanModel = await _context.ResourcePlanModels.FindAsync(id);
            if (resourcePlanModel != null)
            {
                _context.ResourcePlanModels.Remove(resourcePlanModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost, ActionName("GetModulesByProjectId")]
        public JsonResult GetModulesByProjectId(int projectId) {
            List<ModuleModel> modulesList = new List<ModuleModel>();
            if (projectId > 0) {
                modulesList = _context.ModuleModels.Where(m => m.NameId == projectId).ToList();
            }
            return Json(modulesList);
        }
        
        [HttpPost, ActionName("ValidateSave")]
        public JsonResult ValidateSave(ResourcePlanVM requestData)
        {
            bool isAvailable = true;
            var module = _context.ModuleModels.FirstOrDefault(m => m.Id == requestData.ModuleNameId);
            var resource = _context.ResourceModels.Include(t=>t.VacationPlans).FirstOrDefault(r => r.Id == requestData.ResourceNameId);
            if (module != null && resource != null)
            {
                foreach (var singleVacationPlan in resource.VacationPlans)
                {
                    if (singleVacationPlan.StartDate >= module.WorkStartDate && singleVacationPlan.StartDate <= module.WorkEndDate)
                    {
                        isAvailable = false;
                        break;
                    }
                    if (singleVacationPlan.EndDate >= module.WorkStartDate && singleVacationPlan.EndDate <= module.WorkEndDate)
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            
            return Json(new { isAvailable = isAvailable });
        }
        
        [HttpPost, ActionName("Save")]
        public async Task<JsonResult> Save(ResourcePlanVM requestData)
        {
            bool saveSuccessful = false;
            
            var module = _context.ModuleModels.FirstOrDefault(m => m.Id == requestData.ModuleNameId);
            var resource = _context.ResourceModels.FirstOrDefault(r => r.Id == requestData.ResourceNameId);
            var project  = _context.ProjectModels.FirstOrDefault(m => m.Id == requestData.ProjectNameId);
            
            DateTime workStartDate = module.WorkStartDate;
            DateTime workEndDate = module.WorkEndDate;

            while (true)
            {
                // Create a new ResourcePlan entry for each selected month
                var newResourcePlan = new ResourcePlanModel
                {
                    ResourceNameId = requestData.ResourceNameId,
                    ProjectNameId = requestData.ProjectNameId,
                    ModuleNameId = requestData.ModuleNameId,
                    ProjectName = project,
                    ResourceName = resource,
                    Month = workStartDate.Month,
                    Year = workStartDate.Year,
                };
                _context.Add(newResourcePlan);
                
                workStartDate = workStartDate.AddMonths(1);
                if (workStartDate > workEndDate) break;
            }

            int numberOfRowsInserted = await _context.SaveChangesAsync();
            if (numberOfRowsInserted > 0) saveSuccessful = true;
            
            return Json(new { saveSuccessful = saveSuccessful });
        }

        private bool ResourcePlanModelExists(int id)
        {
            return _context.ResourcePlanModels.Any(e => e.Id == id);
        }
    }
}
