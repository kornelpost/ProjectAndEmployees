using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectAndEmployees.Data;
using ProjectAndEmployees.Models;
using ProjectAndEmployees.Models.CompanyViewModels;

namespace ProjectAndEmployees.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectAndEmployeesContext _context;

        public ProjectsController(ProjectAndEmployeesContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index(string searchString, string sortOrder, string currentFilter, int? pageNumber, int? id)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var projects = from s in _context.Projects
                           .Include(s => s.Enrollments)
                                .ThenInclude(s => s.Employee)
                                    select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Title);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Project>.CreateAsync(projects.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Title,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(i => i.Enrollments).ThenInclude(i => i.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            PopulateAssignedCourseData(project);
            return View(project);
        }

        private void PopulateAssignedCourseData(Project project)
        {
            var allEmployee = _context.Employees;
            var projectEmployee = new HashSet<int>(project.Enrollments.Select(c => c.EmployeeId));
            var viewModel = new List<AssignedEmployeeData>();
            foreach (var employee in allEmployee)
            {
                viewModel.Add(new AssignedEmployeeData
                {
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Assigned = projectEmployee.Contains(employee.Id)
                });
            }
            ViewData["Employee"] = viewModel;
        }



        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedEmployee)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectToUpdate = await _context.Projects
            .Include(i => i.Enrollments)
                .ThenInclude(i => i.Employee)
            .FirstOrDefaultAsync(m => m.ProjectId == id);

            if (await TryUpdateModelAsync<Project>(
                projectToUpdate,
                "",
                i => i.Title, i => i.ProjectId, i => i.Description))
            {
                UpdateProjectEmployee(selectedEmployee, projectToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateProjectEmployee(selectedEmployee, projectToUpdate);
            PopulateAssignedCourseData(projectToUpdate);
            return View(projectToUpdate);
        }

        private void UpdateProjectEmployee(string[] selectedEmployee, Project projectToUpdate)
        {
            if (selectedEmployee == null)
            {
                projectToUpdate.Enrollments = new List<Enrollment>();
                return;
            }

            var selectedEmployeeHS = new HashSet<string>(selectedEmployee);
            var projectEmployees = new HashSet<int>
                (projectToUpdate.Enrollments.Select(c => c.Employee.Id));
            foreach (var employee in _context.Employees)
            {
                if (selectedEmployeeHS.Contains(employee.Id.ToString()))
                {
                    if (!projectEmployees.Contains(employee.Id))
                    {
                        projectToUpdate.Enrollments.Add(new Enrollment { ProjectId = projectToUpdate.ProjectId, EmployeeId = employee.Id });
                    }
                }
                else
                {

                    if (projectEmployees.Contains(employee.Id))
                    {
                        Enrollment employeeToRemove = projectToUpdate.Enrollments.FirstOrDefault(i => i.EmployeeId == employee.Id);
                        _context.Remove(employeeToRemove);
                    }
                }
            }
        }


        /*
        private void UpdateProjectEmployee(string[] selectedEmployee, Project projectToUpdate)
        {
            if (selectedEmployee == null)
            {
                projectToUpdate.Enrollments = new List<Enrollment>();
                return;
            }

            var selectedEmployeesHS = new HashSet<string>(selectedEmployee);
            var projectEmployee = new HashSet<int>
                (projectToUpdate.Enrollments.Select(c => c.Employee.Id));
            foreach (var employee in _context.Employees)
            {
                if (selectedEmployeesHS.Contains(employee.Id.ToString()))
                {
                    if (!projectEmployee.Contains(employee.Id))
                    {
                        projectToUpdate.Enrollments.Add(new Enrollment { ProjectId = projectToUpdate.ProjectId, EmployeeId = employee.Id });
                    }
                }
                else
                {

                    if (projectEmployee.Contains(employee.Id))
                    {
                        Enrollment employeeToRemove = projectToUpdate.Enrollments.FirstOrDefault(i => i.EmployeeId == employee.Id);
                        _context.Remove(employeeToRemove);
                    }
                }
            }
        }

        */


        /*
        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Title,Description")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            return View(project);
        }

        */

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
