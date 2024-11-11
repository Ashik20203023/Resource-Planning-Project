using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tesvolt.dbmodels.app;
using X.PagedList.Extensions;

namespace tesvolt.web.app.Controllers
{
    public class ModuleModelController : Controller
    {
        private readonly TesvoltDbContext _context;

        public ModuleModelController(TesvoltDbContext context)
        {
            _context = context;
        }

        // GET: ModuleModel
        // GET: ModuleModel
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; // Number of records per page
            int pageNumber = page ?? 1; // Default to page 1 if not specified

            // Retrieve ModuleModels with related data (e.g., Project names)
            var moduleModels = _context.ModuleModels
                .Include(m => m.Name) // Include ProjectModel (assuming 'Name' is the navigation property)
                .OrderBy(m => m.Modules) // Order by module name or any other desired property
                .ToPagedList(pageNumber, pageSize); // Use ToPagedList for pagination

            return View(moduleModels);
        }


        // GET: ModuleModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleModel = await _context.ModuleModels
                .Include(m => m.Name)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moduleModel == null)
            {
                return NotFound();
            }

            return View(moduleModel);
        }

        // GET: ModuleModel/Create
        public IActionResult Create()
        {
            ViewData["NameId"] = new SelectList(_context.ProjectModels, "Id", "Name");

            return View();
        }

        // POST: ModuleModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Modules,NameId,WorkStartDate,WorkEndDate,NoOfDaysNeeded")] ModuleModel moduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NameId"] = new SelectList(_context.ProjectModels, "Id", "AssetName", moduleModel.NameId);
            return View(moduleModel);
        }

        // GET: ModuleModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleModel = await _context.ModuleModels.FindAsync(id);
            if (moduleModel == null)
            {
                return NotFound();
            }
            ViewData["NameId"] = new SelectList(_context.ProjectModels, "Id", "Name", moduleModel.NameId);

            return View(moduleModel);
        }

        // POST: ModuleModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Modules,NameId,WorkStartDate,WorkEndDate,NoOfDaysNeeded")] ModuleModel moduleModel)
        {
            if (id != moduleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleModelExists(moduleModel.Id))
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
            ViewData["NameId"] = new SelectList(_context.ProjectModels, "Id", "AssetName", moduleModel.NameId);
            return View(moduleModel);
        }

        // GET: ModuleModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleModel = await _context.ModuleModels
                .Include(m => m.Name)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moduleModel == null)
            {
                return NotFound();
            }

            return View(moduleModel);
        }

        // POST: ModuleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moduleModel = await _context.ModuleModels.FindAsync(id);
            if (moduleModel != null)
            {
                _context.ModuleModels.Remove(moduleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleModelExists(int id)
        {
            return _context.ModuleModels.Any(e => e.Id == id);
        }
    }
}
