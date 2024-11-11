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
    public class ResourceController : Controller
    {
        private readonly TesvoltDbContext _context;

        public ResourceController(TesvoltDbContext context)
        {
            _context = context;
        }

        // GET: Resource
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; // Number of records per page
            int pageNumber = page ?? 1; // Default to page 1 if no page is specified

            var resourceModels = _context.ResourceModels
                //.Include(r => r.Category)   // Include related Category entity
                //.Include(r => r.Role)       // Include related Role entity
                .OrderBy(r => r.Name)       // Sorting by resource name
                .ToPagedList(pageNumber, pageSize); // Apply pagination

            return View(resourceModels);
        }


        // GET: Resource/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourceModel = await _context.ResourceModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resourceModel == null)
            {
                return NotFound();
            }

            return View(resourceModel);
        }

        // GET: Resource/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resource/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Role,Skills")] ResourceModel resourceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resourceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resourceModel);
        }

        // GET: Resource/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourceModel = await _context.ResourceModels.FindAsync(id);
            if (resourceModel == null)
            {
                return NotFound();
            }
            return View(resourceModel);
        }

        // POST: Resource/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Role,Skills")] ResourceModel resourceModel)
        {
            if (id != resourceModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resourceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourceModelExists(resourceModel.Id))
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
            return View(resourceModel);
        }

        // GET: Resource/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resourceModel = await _context.ResourceModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resourceModel == null)
            {
                return NotFound();
            }

            return View(resourceModel);
        }

        // POST: Resource/Delete/5
        // POST: Resource/Delete/5
        // POST: Resource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resourceModel = await _context.ResourceModels.FindAsync(id);
            if (resourceModel != null)
            {
                _context.ResourceModels.Remove(resourceModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }




        private bool ResourceModelExists(int id)
        {
            return _context.ResourceModels.Any(e => e.Id == id);
        }
    }
}
