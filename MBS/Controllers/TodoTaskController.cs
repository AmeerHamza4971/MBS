using MBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBS.Controllers
{
    public class TodoTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TodoTask
        public async Task<IActionResult> TodoList()
        {
            var tasks = await _context.TodoTasks
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();

            return View(tasks);
        }

        // GET: TodoTask/TodoDetail/5
        public async Task<IActionResult> TodoDetail(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTasks
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (todoTask == null)
            {
                return NotFound();
            }

            // Get related billings for tags
            var billingIds = todoTask.TagList
                .Where(t => long.TryParse(t, out _))
                .Select(t => long.Parse(t))
                .ToList();

            ViewBag.RelatedBillings = await _context.Billings
                .Where(b => billingIds.Contains(b.Id))
                .ToListAsync();

            return View(todoTask);
        }

        // GET: TodoTask/CreateTodo
        public async Task<IActionResult> CreateTodo()
        {
            // Get all billings for tag selection
            ViewBag.Billings = await _context.Billings.ToListAsync();
            ViewBag.TaskTypes = new List<string> { "Work", "Personal", "Shopping", "Health", "Other" };
            ViewBag.TaskStatuses = new List<string> { "Pending", "Active", "Inactive", "Completed" };

            return View();
        }

        // POST: TodoTask/CreateTodo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTodo(TodoTask todoTask, string[] selectedTags)
        {
            if (todoTask != null)
            {
                todoTask.CreatedDate = DateTime.Now;

                // Handle tags
                if (selectedTags != null && selectedTags.Length > 0)
                {
                    todoTask.Tags = string.Join(",", selectedTags);
                }

                await _context.TodoTasks.AddAsync(todoTask);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(TodoList));
            }

            // If we got this far, something failed, redisplay form
            // Debug information for validation errors
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new {
                    Key = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                })
                .ToList();

            ViewBag.ValidationErrors = errors;

            // Repopulate form data
            ViewBag.Billings = await _context.Billings.ToListAsync();
            ViewBag.TaskTypes = new List<string> { "Work", "Personal", "Shopping", "Health", "Other" };
            ViewBag.TaskStatuses = new List<string> { "Pending", "Active", "Inactive", "Completed" };

            return View(todoTask);
        }

        // GET: TodoTask/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTasks.FindAsync(id);

            if (todoTask == null || todoTask.IsDeleted)
            {
                return NotFound();
            }

            ViewBag.Billings = await _context.Billings.ToListAsync();
            ViewBag.TaskTypes = new List<string> { "Work", "Personal", "Shopping", "Health", "Other" };
            ViewBag.TaskStatuses = new List<string> { "Pending", "Active", "Inactive", "Completed" };
            ViewBag.SelectedTags = todoTask.TagList;

            return View(todoTask);
        }

        // POST: TodoTask/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TodoTask todoTask, string[] selectedTags)
        {
            if (id != todoTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTask = await _context.TodoTasks.AsNoTracking()
                        .FirstOrDefaultAsync(t => t.Id == id);

                    if (existingTask == null || existingTask.IsDeleted)
                    {
                        return NotFound();
                    }

                    todoTask.CreatedDate = existingTask.CreatedDate;
                    todoTask.UpdatedDate = DateTime.Now;

                    // Handle tags
                    if (selectedTags != null && selectedTags.Length > 0)
                    {
                        todoTask.Tags = string.Join(",", selectedTags);
                    }
                    else
                    {
                        todoTask.Tags = null;
                    }

                    _context.Update(todoTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoTaskExists(todoTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(TodoList));
            }

            ViewBag.Billings = await _context.Billings.ToListAsync();
            ViewBag.TaskTypes = new List<string> { "Work", "Personal", "Shopping", "Health", "Other" };
            ViewBag.TaskStatuses = new List<string> { "Pending", "Active", "Inactive", "Completed" };
            ViewBag.SelectedTags = selectedTags;

            return View(todoTask);
        }

        // POST: TodoTask/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var todoTask = await _context.TodoTasks.FindAsync(id);

            if (todoTask == null)
            {
                return NotFound();
            }

            // Soft delete
            todoTask.IsDeleted = true;
            todoTask.UpdatedDate = DateTime.Now;

            _context.Update(todoTask);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        private bool TodoTaskExists(long id)
        {
            return _context.TodoTasks.Any(e => e.Id == id);
        }
    }
}
