using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.Models;

namespace SupportCore.Controllers
{
    [Authorize(Roles = "Администратор,Сотрудник,Менеджер")]
    public class SLAsController : Controller
    {
        private readonly Context _context;

        public SLAsController(Context context)
        {
            _context = context;
        }

        // GET: SLAs
        public async Task<IActionResult> Index()
        {
            var context = _context.SLAs.AsNoTracking().Where(s => s.Type == 0);
            return PartialView(await context.ToListAsync());
        }
        // GET: SLAs
        public IActionResult IndexRules(int ParentId)
        {
            var sLa = new SLA
            {
                ParentId = ParentId
            };
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["FieldId"] = new SelectList(_context.Fields.AsNoTracking().Include(n => n.Form).Where(f=>f.Type=="select"&&f.Form.Type==0), "Id", "Name");
            return PartialView(sLa);
        }

        // GET: SLAs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sLA = await _context.SLAs
                .Include(s => s.Category)
                .Include(s => s.Field)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sLA == null)
            {
                return NotFound();
            }

            return View(sLA);
        }

        // GET: SLAs/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            //ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Name");
            return PartialView();
        }

        // POST: SLAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsDefault,Type,DeadTime,ResponseTime,ParentId,FieldId,FieldValue,CategoryId")] SLA sLA,int step)
        {
            
            if (ModelState.IsValid)
            {

                if (sLA.Type != 0 && sLA.CategoryId!=null && _context.SLAs.AsNoTracking().FirstOrDefaultAsync(s => s.CategoryId == sLA.CategoryId && s.ParentId == sLA.ParentId).Result!=null) return new StatusCodeResult(500);
                _context.Add(sLA);
                await _context.SaveChangesAsync();
                if (sLA.ParentId != null) return RedirectToAction(nameof(IndexRules), new { sLA.ParentId });
                return RedirectToAction(nameof(IndexRules), new { ParentId = sLA.Id });
            }

            ViewData["CategoryId"] = new SelectList(_context.Category.AsNoTracking(), "Id", "Name", sLA.CategoryId);
            ViewData["FieldId"] = new SelectList(_context.Fields.AsNoTracking(), "Id", "Name", sLA.FieldId);            
            return PartialView(sLA);
        }

        // GET: SLAs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sLA = await _context.SLAs.FindAsync(id);
            if (sLA == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", sLA.CategoryId);
            ViewData["FieldId"] = new SelectList(_context.Fields.Include(n => n.Form).AsNoTracking().Where(f => f.Type == "select" && f.Form.Type == 0), "Id", "Name");
            return PartialView(sLA);
        }

        // POST: SLAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsDefault,Type,DeadTime,ResponseTime,ParentId,FieldId,FieldValue,CategoryId")] SLA sLA)
        {
            if (id != sLA.Id)
            {
                return NotFound();
            }
            if(sLA.ParentId != null)
            {
                sLA.Type = 1;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sLA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SLAExists(sLA.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category.AsNoTracking(), "Id", "Name", sLA.CategoryId);
            ViewData["FieldId"] = new SelectList(_context.Fields.AsNoTracking(), "Id", "Name", sLA.FieldId);
            return View(sLA);
        }

        // GET: SLAs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sLA = await _context.SLAs
                .Include(s => s.Category)
                .Include(s => s.Field)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sLA == null)
            {
                return NotFound();
            }

            return View(sLA);
        }

        // POST: SLAs/Delete/5
        [HttpPost, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? ParentId)
        {
            var sLA = await _context.SLAs.FindAsync(id);
            _context.SLAs.Remove(sLA);
            await _context.SaveChangesAsync();
            if(ParentId!=null) return RedirectToAction(nameof(IndexRules),new { ParentId });
            return RedirectToAction(nameof(Index));
        }

        private bool SLAExists(int id)
        {
            return _context.SLAs.Any(e => e.Id == id);
        }
        public JsonResult GetValues(int FieldId)
        {
            var ValueList = _context.Fields.AsNoTracking().SingleOrDefaultAsync(f => f.Id == FieldId).Result.Configuration.Split("\r\n");
            return Json(ValueList);
        }
        public async Task<JsonResult> GetFields()
        {
            var ValueList = await _context.Fields.AsNoTracking()
                .AsNoTracking().Where(f => f.Type == "select" && f.Form.Type == 0)
                .Select(r => new
                {
                    r.Id,
                    r.Name
                }).ToListAsync();
            return Json(ValueList);
        }
    }
}
