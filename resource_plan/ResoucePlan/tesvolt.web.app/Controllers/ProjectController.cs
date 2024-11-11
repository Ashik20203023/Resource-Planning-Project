using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tesvolt.dbmodels.app;
using X.PagedList;
using X.PagedList.Extensions;

namespace tesvolt.web.app.Controllers
{
    public class ProjectController : Controller
    {
        private readonly TesvoltDbContext _context;

        public ProjectController(TesvoltDbContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; // Number of records per page
            int pageNumber = page ?? 1; // Default to page 1 if not specified

            // Retrieve ProjectModels with related data (if needed)
            var projectModels = _context.ProjectModels
                .Include(p => p.PoName) // Assuming you have a PoName relationship in your ProjectModel
                //.Include(p => p.AccumulatedHr)
                .OrderBy(p => p.Name) // Order by project name
                .ToPagedList(pageNumber, pageSize);

            return View(projectModels);
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModels
                //.Include(p => p.AccumulatedHr)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // GET: Project/Create
        //public async IActionResult Create()
        //{
        //    //    var resourceList = new List<ResourceModel>
        //    //{
        //    //    new ResourceModel { Id = 1, Name = "John Doe", Category = ResourceCategory.Internal, Role = ResourcRole.Developer, Skills = "C#, ASP.NET Core" },
        //    //    new ResourceModel { Id = 2, Name = "Jane Smith", Category = ResourceCategory.External, Role = ResourcRole.ProductOwner, Skills = "Project Management, Agile" }
        //    //    // Add more resources as needed
        //    //};
        //    var resourceList= new List<ResourceModel>();
        //    resourceList= _context.ResourceModels.ToList();
        //    // Passing the list to the ViewBag for simplicity, you can also use ViewData or a ViewModel
        //    ViewBag.ResourceList = resourceList.Select(x => new { Id = x.Id, Name = x.Name }).ToList();

        //    return View();
        //}
        public async Task<IActionResult> Create()
        {
            // Fetch all resources where the role is 'ProductOwner'
            var productOwners = await _context.ResourceModels
                .Where(r => r.Role == ResourceRole.ProductOwner)
                .ToListAsync();
            //var accumulatedHrs = await _context.ResourceModels
            // .Where(r => r.Role == ResourceRole.HR)
            // .ToListAsync();


            ViewBag.ProductOwners = new SelectList(productOwners, "Id", "Name");
            //ViewBag.AccumulatedHrs = new SelectList(accumulatedHrs, "Id", "Name");
            return View();
        }



        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,ProjectCode,Name,AssetName,AccumulatedHr,ProjectStatus")] ProjectModel projectModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(projectModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(projectModel);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectCode,Name,AssetName,AccumulatedHrId,ProjectStatus,PoNameId")] ProjectModel projectModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the selected ResourceModel based on the PoNameId
                projectModel.PoName = await _context.ResourceModels.FindAsync(projectModel.PoNameId);
                //projectModel.AccumulatedHr = await _context.ResourceModels.FindAsync(projectModel.AccumulatedHrId);

                // Add the projectModel to the context and save changes
                _context.Add(projectModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productOwners = await _context.ResourceModels
                .Where(r => r.Role == ResourceRole.ProductOwner)
                .ToListAsync();
            //var accumulatedHrs = await _context.ResourceModels
            //    .Where(r => r.Role == ResourceRole.HR)
            //    .ToListAsync();

            ViewBag.ProductOwners = new SelectList(productOwners, "Id", "Name");
            //ViewBag.AccumulatedHrs = new SelectList(accumulatedHrs, "Id", "Name");
            var projectModel = await _context.ProjectModels.FindAsync(id);
            if (projectModel == null)
            {
                return NotFound();
            }
            return View(projectModel);
        }


        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectCode,Name,AssetName,AccumulatedHrId,ProjectStatus,PoNameId")] ProjectModel projectModel)
        {
            if (id != projectModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    projectModel.PoName = await _context.ResourceModels.FindAsync(projectModel.PoNameId);
                    //projectModel.AccumulatedHr = await _context.ResourceModels.FindAsync(projectModel.AccumulatedHrId);
                    _context.Update(projectModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectModelExists(projectModel.Id))
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
            return View(projectModel);
        }



        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModels
                 .Include(p => p.PoName) // If you need to show related data
                //.Include(p => p.AccumulatedHr)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // POST: Project/Delete/5
        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectModel = await _context.ProjectModels.FindAsync(id);
            if (projectModel != null)
            {
                _context.ProjectModels.Remove(projectModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectModelExists(int id)
        {
            return _context.ProjectModels.Any(e => e.Id == id);
        }
    }
}