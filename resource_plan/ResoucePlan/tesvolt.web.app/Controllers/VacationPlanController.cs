using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tesvolt.dbmodels.app;
using Microsoft.EntityFrameworkCore;
using tesvolt.web.app;
using X.PagedList;
using X.PagedList.Extensions;





namespace tesvolt.web.app.Controllers
{

    // GET: VacationPlan
    public class VacationPlanController : Controller
    {
        private readonly TesvoltDbContext _context;

        public VacationPlanController(TesvoltDbContext context)
        {
            _context = context;
        }


        // GET: VacationPlan
        //public async Task<IActionResult> Index(int? page)
        //{
        //    int pageSize = 10; // Number of records per page
        //    int pageNumber = page ?? 1; // Default to page 1 if not specified

        //    var vacationPlans = await _context.VacationPlanModels
        //        .Include(v => v.ResourseName) // Include related ResourceName
        //        .OrderBy(v => v.ResourseName.Name) // Ensure this is correct
        //        .ToPagedListAsync(pageNumber, pageSize); // Apply pagination

        //    return View(vacationPlans);
        //}
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; // Number of records per page
            int pageNumber = page ?? 1; // Default to page 1 if no page is specified

            var vacationPlans = _context.VacationPlanModels
               .Include(v => v.ResourceModel)   // Include related ResourceModel entity
                                                //.Include(r => r.Role)         // Include related Role entity if needed
     .OrderByDescending(r => r.StartDate)       // First sort by StartDate
     .ThenBy(r => r.ResourceModel.Name) // Then sort by ResourceName
     .ToPagedList(pageNumber, pageSize); // Apply pagination // Apply pagination

            return View(vacationPlans);
        }
        // GET: VacationPlan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationPlanModel = await _context.VacationPlanModels
                .Include(v => v.ResourceModel) // Correctly include the ResourceModel navigation property
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vacationPlanModel == null)
            {
                return NotFound();
            }

            return View(vacationPlanModel);
        }



        // GET: VacationPlan/Create
        public async Task<IActionResult> Create()
        {
            var vacationPlanModel = new VacationPlanModel();



            ViewData["ResourceModelId"] = new SelectList(_context.ResourceModels, "Id", "Name");

            return View(vacationPlanModel);


        }

        // POST: VacationPlan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceModelId,StartDate,EndDate")] VacationPlanModel vacationPlanModel)
        {
            // Calculate NoOfDay only if both dates are provided
            if (vacationPlanModel.StartDate != null && vacationPlanModel.EndDate != null)
            {
                vacationPlanModel.NoOfDay = (vacationPlanModel.EndDate - vacationPlanModel.StartDate).Days + 1; // Include both StartDate and EndDate
            }

            // Check if the total vacation days for this resource in the current year exceed 20
            int year = vacationPlanModel.StartDate.Year;
            var totalDaysTaken = await _context.VacationPlanModels
                .Where(v => v.ResourceModelId == vacationPlanModel.ResourceModelId && v.StartDate.Year == year)
                .SumAsync(v => v.NoOfDay);

            if (totalDaysTaken + vacationPlanModel.NoOfDay > 20)
            {
                ModelState.AddModelError("", "A resource cannot take more than 20 days of vacation in a year.");
                ViewData["ResourceModelId"] = new SelectList(_context.ResourceModels, "Id", "Name", vacationPlanModel.ResourceModelId);
                return View(vacationPlanModel); // Return to the form with an error message
            }

            if (ModelState.IsValid)
            {
                _context.Add(vacationPlanModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we reach here, there was a validation error
            ViewData["ResourceModelId"] = new SelectList(_context.ResourceModels, "Id", "Name", vacationPlanModel.ResourceModelId);
            return View(vacationPlanModel);
        }





        // GET: VacationPlan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationPlanModel = await _context.VacationPlanModels
                .Include(v => v.ResourceModel) // Correctly include the ResourceModel navigation property
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationPlanModel == null)
            {
                return NotFound();
            }
            ViewData["ResourceModelId"] = new SelectList(_context.ResourceModels, "Id", "Name", vacationPlanModel.ResourceModelId);
            return View(vacationPlanModel);
        }


        // POST: VacationPlan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceModelId,StartDate,EndDate")] VacationPlanModel vacationPlanModel)
        {
            if (id != vacationPlanModel.Id)
            {
                return NotFound();
            }

            // Calculate NoOfDay only if both dates are provided
            if (vacationPlanModel.StartDate != null && vacationPlanModel.EndDate != null)
            {
                vacationPlanModel.NoOfDay = (vacationPlanModel.EndDate - vacationPlanModel.StartDate).Days + 1; // Include both StartDate and EndDate
            }

            // Check if the total vacation days for this resource in the current year exceed 20
            int year = vacationPlanModel.StartDate.Year;
            var totalDaysTaken = await _context.VacationPlanModels
                .Where(v => v.ResourceModelId == vacationPlanModel.ResourceModelId && v.StartDate.Year == year && v.Id != vacationPlanModel.Id)
                .SumAsync(v => v.NoOfDay);

            if (totalDaysTaken + vacationPlanModel.NoOfDay > 20)
            {
                ModelState.AddModelError("", "A resource cannot take more than 45 days of vacation in a year.");
                ViewData["ResourceModelId"] = new SelectList(_context.ResourceModels, "Id", "Name", vacationPlanModel.ResourceModelId);
                return View(vacationPlanModel); // Return to the form with an error message
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacationPlanModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationPlanModelExists(vacationPlanModel.Id))
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

            ViewData["ResourceModelId"] = new SelectList(_context.ResourceModels, "Id", "Name", vacationPlanModel.ResourceModelId);
            return View(vacationPlanModel);
        }


        // GET: VacationPlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationPlanModel = await _context.VacationPlanModels
                .Include(v => v.ResourceModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationPlanModel == null)
            {
                return NotFound();
            }

            return View(vacationPlanModel);
        }

        // POST: VacationPlan/Delete/5
        // POST: VacationPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacationPlanModel = await _context.VacationPlanModels.FindAsync(id);
            if (vacationPlanModel != null)
            {
                _context.VacationPlanModels.Remove(vacationPlanModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



        private bool VacationPlanModelExists(int id)
        {
            return _context.VacationPlanModels.Any(e => e.Id == id);
        }
    }
}
