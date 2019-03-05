using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.Components;
using SupportCore.Models;

namespace SupportCore.Controllers
{
    [Authorize(Roles = "Администратор,Сотрудник,Менеджер")]
    public class FormsController : Controller
    {
        private readonly Context _context;

        public FormsController(Context context)
        {
            _context = context;
        }

        // GET: Forms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Forms.AsNoTracking().ToListAsync());
        }

        // POST: Forms/FieldDelete
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> FieldDelete(int id, int FormId)
        {
            var field = await _context.Fields.SingleOrDefaultAsync(m => m.Id == id);
            var formev = _context.FormEntryValues
                .Include(f=>f.Field)
                .Where(f => f.FieldId == id);
            _context.RemoveRange(formev);
            _context.Remove(field);
            await _context.SaveChangesAsync();
            return RedirectToAction("Fields", new { id = FormId });
            // return ViewComponent("Fields",FormId);
        }
        // POST: Forms/FieldDelete
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public IActionResult Fields(int id)
        {
            return PartialView(new Field { FormId = id });
            // return ViewComponent("Fields",FormId);
        }
        //Form for configuration field
        [HttpGet]
        public IActionResult FieldConfig(int id)
        {
            var field = _context.Fields
                .AsNoTracking()
                .SingleOrDefault(f => f.Id == id);
            return PartialView(field);
        }
        //Edit configuration field
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FieldConfig(Field field)
        {
            var fieldforUpdate = await _context.Fields
                .SingleOrDefaultAsync(f => f.Id == field.Id);
            fieldforUpdate.Configuration = field.Configuration;
            _context.Update(fieldforUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                    return Ok("Сохранено");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            return PartialView(fieldforUpdate);
        }
        //Add new field to form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fields([Bind("Type,Name,Required,FormId")]Field field)
        {
            field.DateCreate = DateTime.Now;
            _context.Add(field);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Fields), new { id = field.FormId });
            // return ViewComponent("Fields",FormId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveField(int? id,bool Required)
        {
            if (id == null)
            {
                return NotFound();
            }
            var field = await _context.Fields.SingleOrDefaultAsync(n => n.Id == id);
            if (ModelState.IsValid)
            {
                try
                {
                    field.Name = HttpContext.Request.Form["item.Name"];
                    if(field.Type!= HttpContext.Request.Form["item.Type"]) { 
                        field.Type = HttpContext.Request.Form["item.Type"];
                        field.Configuration = "";
                    }
                    field.Required = Required;
                    _context.Update(field);
                    await _context.SaveChangesAsync();
                    return Ok("Cохранено");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return RedirectToAction("Fields", new { id = field.FormId });
        }
           
        // GET: Forms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = await _context.Forms
                .SingleOrDefaultAsync(m => m.Id == id);
            if (form == null)
            {
                return NotFound();
            }

            return View(form);
        }

        // GET: Forms/Create
        public IActionResult Create()
        {

            return PartialView();
        }

        // POST: Forms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Discription")] Models.Form form)
        {
            if (ModelState.IsValid)
            {
                form.DateCreate = DateTime.Now;
                _context.Add(form);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Fields), new { id = form.Id });
            }
            return View(form);
        }

        // GET: Forms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = await _context.Forms.SingleOrDefaultAsync(m => m.Id == id);
            if (form == null)
            {
                return NotFound();
            }
            return View(form);
        }

        // POST: Forms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Name,Label,Discription,DateCreate,DateUpdate")] Models.Form form)
        {
            if (id != form.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    form.Type = 1;
                    _context.Update(form);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormExists(form.Id))
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
            return View(form);
        }

        // GET: Forms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = await _context.Forms
                .SingleOrDefaultAsync(m => m.Id == id);
            if (form == null)
            {
                return NotFound();
            }

            return PartialView(form);
        }

        // POST: Forms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var form = await _context.Forms.Include(f => f.Fields).SingleOrDefaultAsync(m => m.Id == id);
            _context.Forms.Remove(form);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormExists(int id)
        {
            return _context.Forms.Any(e => e.Id == id);
        }
    }
}
