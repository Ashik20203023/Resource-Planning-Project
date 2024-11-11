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
    public class AttendanceEntryController : Controller
    {
        private readonly TesvoltDbContext _context;

        public AttendanceEntryController(TesvoltDbContext context)
        {
            _context = context;
        }

        // GET: AttendanceEntry
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; // Number of records per page
            int pageNumber = page ?? 1; // Default to page 1 if not specified

            var attendanceEntries = _context.AttendanceEntryModels
                .Include(a => a.ProjectName)   // Include related ProjectName
                .Include(a => a.ResourceName)  // Include related ResourceName
                .OrderBy(a => a.StartDate)
                .ThenBy(a => a.ResourceName.Name)  // Order by ResourceName
                .ToPagedList(pageNumber, pageSize); // Apply pagination

            return View(attendanceEntries);
        }


        // GET: AttendanceEntry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceEntryModel = await _context.AttendanceEntryModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendanceEntryModel == null)
            {
                return NotFound();
            }

            return View(attendanceEntryModel);
        }

        // GET: AttendanceEntry/Create
        public async Task<IActionResult> Create()
        {
            // Asynchronously retrieve the list of ResourceModel objects from the database
            var resourceList = await _context.ResourceModels.ToListAsync();

            // Passing the list to the ViewBag for simplicity, you can also use ViewData or a ViewModel
            ViewBag.ResourceList = resourceList.Select(x => new { Id = x.Id, Name = x.Name }).ToList();

            var projectList = await _context.ProjectModels.ToListAsync();

            // Passing the list to the ViewBag for simplicity, you can also use ViewData or a ViewModel
            ViewBag.projectList = projectList.Select(x => new { Id = x.Id, Name = x.Name }).ToList();

            ViewBag.TaskType = Enum.GetValues(typeof(TaskType)).Cast<TaskType>().Select(v => v.ToString()).ToList();
            return View();
        }
        /*  public async Task<IActionResult> Create()
          {// Asynchronously retrieve the list of ResourceModel objects from the database
              var resourceList = _context.ResourceModels.ToListAsync();

              // Passing the list to the ViewBag for simplicity, you can also use ViewData or a ViewModel
              ViewBag.ResourceList = resourceList.Select(x => new { Id = x.Id, Name = x.Name }).ToList();


              return View();
          }*/

        // POST: AttendanceEntry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: AttendanceEntry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceNameId,StartDate,EndDate,ProjectNameId,TaskType")] AttendanceEntryModel attendanceEntryModel)
        {
            if (ModelState.IsValid)
            {
                attendanceEntryModel.ResourceName = await _context.ResourceModels.FindAsync(attendanceEntryModel.ResourceNameId);
                attendanceEntryModel.ProjectName = await _context.ProjectModels.FindAsync(attendanceEntryModel.ProjectNameId);

                _context.Add(attendanceEntryModel);
                await _context.SaveChangesAsync();

                List<AttendanceDetailEntryModel> detailEntries = new List<AttendanceDetailEntryModel>();

                for (var date = attendanceEntryModel.StartDate; date <= attendanceEntryModel.EndDate; date = date.AddDays(1))
                {
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var detailEntry = new AttendanceDetailEntryModel
                        {
                            ResourceNameId = attendanceEntryModel.ResourceNameId,
                            AttendanceDate = date,
                            AttendanceEntry = attendanceEntryModel, // Link to the parent entry
                            ProjectNameId = attendanceEntryModel.ProjectNameId,
                            TaskType = attendanceEntryModel.TaskType
                        };
                        detailEntries.Add(detailEntry);
                    }
                }

                _context.AddRange(detailEntries);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed; redisplay form
            return View(attendanceEntryModel);
        }




        // GET: AttendanceEntry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceEntryModel = await _context.AttendanceEntryModels.FindAsync(id);
            if (attendanceEntryModel == null)
            {
                return NotFound();
            }

            return View(attendanceEntryModel);
        }

        // POST: AttendanceEntry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,TaskType")] AttendanceEntryModel attendanceEntryModel)
        {
            if (id != attendanceEntryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEntry = await _context.AttendanceEntryModels.FindAsync(id);
                    if (existingEntry == null)
                    {
                        return NotFound();
                    }

                    // Only update the fields that can change
                    existingEntry.StartDate = attendanceEntryModel.StartDate;
                    existingEntry.EndDate = attendanceEntryModel.EndDate;
                    existingEntry.TaskType = attendanceEntryModel.TaskType;

                    _context.Update(existingEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceEntryModelExists(attendanceEntryModel.Id))
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
            return View(attendanceEntryModel);
        }


        // GET: AttendanceEntry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceEntryModel = await _context.AttendanceEntryModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendanceEntryModel == null)
            {
                return NotFound();
            }

            return View(attendanceEntryModel);
        }

        // POST: AttendanceEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendanceEntryModel = await _context.AttendanceEntryModels.FindAsync(id);
            if (attendanceEntryModel != null)
            {
                _context.AttendanceEntryModels.Remove(attendanceEntryModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceEntryModelExists(int id)
        {
            return _context.AttendanceEntryModels.Any(e => e.Id == id);
        }
    }
}
