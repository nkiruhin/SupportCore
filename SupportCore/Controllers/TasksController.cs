using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.App.Classes;
using SupportCore.App.Interfaces;
using SupportCore.Models;

namespace SupportCore.Controllers
{
    public class TasksController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        public TasksController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(int? TicketId,int edit=1)
        {
            if (TicketId == null) { return PartialView(await _context.Tasks
                .Include(s=>s.Staff)
                .Include(o=>o.Organization)
                .AsNoTracking().ToListAsync()); }
            Ticket ticket = new Ticket() { Id = (int)TicketId };
            ticket.Tasks = await _context.Tasks.AsNoTracking()
                .Where(n => n.TicketId == TicketId)
                .Include(s => s.Staff)
                .Include(o => o.Organization)
                .ToListAsync();
            ViewBag.Edit = edit;
            return PartialView(ticket);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.AsNoTracking()
                .Include(s => s.Staff)
                .Include(o => o.Organization)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return PartialView(task);
        }

        // GET: Tasks/Create
        public IActionResult Create(int Id)
        {
            Tasks tasks = new Tasks { TicketId = Id };
            return PartialView(tasks);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Body,Number,TicketId,StaffId,OrganizationId")] Tasks task)
        {
            if (ModelState.IsValid)
            {
                task.DateCreate = DateTime.Now;
                var Person = await CurrentPersonAsync();
                string staffName = "";
                string orgName = "";
                if (task.OrganizationId != null)
                {
                    task.Type = 1;
                }
                if (task.OrganizationId!=null) orgName = _context.Organizations.AsNoTracking().SingleOrDefault(n => n.Id == task.OrganizationId).Name;
                if (task.StaffId!=null) staffName= _context.Person.AsNoTracking().SingleOrDefault(n => n.Id == task.StaffId).Name;
                var thread=new TicketThread()
                {
                    TicketId = task.TicketId,
                    DateCreate = DateTime.Now,
                    Title = $"Поставлена задача",
                    Body = $"Поставлена задача {task.Number} на {orgName} {staffName}",
                    Poster=Person.Name,
                    Type=0                    
                };
                var person = await CurrentPersonAsync();
                if (!String.IsNullOrEmpty(person.ApiKey))
                {
                    RedmineService redmine = new RedmineService(_context, person.ApiKey);
                    ReadmineIssue issueInfo = new ReadmineIssue();
                    await redmine.CreateIssueAsync(task);
                    return PartialView(issueInfo);
                }
                _context.Add(task);
                _context.Add(thread);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new { task.TicketId});
            }
            return PartialView(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.AsNoTracking()
                .Include(o=>o.Organization)
                .Include(p=>p.Staff)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return PartialView(task);
        }
        // GET: Tasks/Close/5
        public async Task<IActionResult> Close(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            task.DateClose = DateTime.Now;
            return PartialView(task);
        }
        // POST: Tasks/Close/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var taskToUpdate = await _context.Tasks.SingleOrDefaultAsync(t => t.Id == id); 
                var Person = await CurrentPersonAsync();
                taskToUpdate.Status = 1;
                await TryUpdateModelAsync<Tasks>(taskToUpdate,"",t=>t.Result,t=>t.DateClose,t=>t.Status);
                var thread = new TicketThread()
                {
                    TicketId = taskToUpdate.TicketId,
                    DateCreate = DateTime.Now,
                    Title = $"Закрыта задача",
                    Body = $@"<p>Задача {taskToUpdate.Number}/{taskToUpdate.Id} выполнена {taskToUpdate.DateClose}</p>
                    Результат выполнения: {taskToUpdate.Result}
                    ",
                    Poster = Person.Name,
                    Type = 0
                };
                _context.Add(thread);
                await _context.SaveChangesAsync();     
                return RedirectToAction(nameof(Index),new { Ticketid= taskToUpdate.TicketId });
            }
            return PartialView();
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreate,DateClose,Title,Body,Number,Status,Type")] Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .SingleOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(m => m.Id == id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Tasks/GetIssue/5
        public async Task<IActionResult> GetIssue(string id)
        {
           
            var person = await CurrentPersonAsync();
            if (!String.IsNullOrEmpty(person.ApiKey)) {
                RedmineService redmine = new RedmineService(_context, person.ApiKey);
                ReadmineIssue issueInfo = new ReadmineIssue();
                issueInfo =await redmine.ReadIssueAsync(id);
                return PartialView(issueInfo);
            }
            return PartialView(null);
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
        private async Task<Person> CurrentPersonAsync()
        {
            var person = await _context.Person.AsNoTracking().
                SingleOrDefaultAsync(p => p.AccountID == _userManager.GetUserId(HttpContext.User));
            return person;
        }
    }
}
