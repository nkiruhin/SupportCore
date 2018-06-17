using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.App.Interfaces;
using SupportCore.Models;
using SupportCore.ViewModels;

namespace SupportCore.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly Context _context;

        public TemplatesController(Context context)
        {
            _context = context;
        }

        // GET: Templates
        public async Task<IActionResult> Index()
        {
            var events = new Event().EventList().ToList();
            for (int i=0 ; i<events.Count ;i++)
            {
                ViewData[events[i].Value] = events[i].Text;
            }

            return View(await _context.Templates.AsNoTracking().ToListAsync());
        }

        //// GET: Templates/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var template = await _context.Templates
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (template == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(template);
        //}

        // GET: Templates/Create
        public IActionResult Create()
        {
            ViewData["Events"] = new Event().EventList();
            return PartialView();
        }

        // POST: Templates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventType,Body,Name,IsDefault")] Template template)
        {
            if (ModelState.IsValid)
            {
                _context.Add(template);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(template);
        }

        // GET: Templates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Events"] = new Event().EventList();
            var template = await _context.Templates.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }
            return View(template);
        }
        // GET: Templates/Edit/5
        public async Task<string> GetTemplate(int? id,int TicketId)
        {
            return await new Templates(_context).GetTemplate(id, TicketId);
        }
        // POST: Templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventType,Body,Name,IsDefault")] Template template)
        {
            if (id != template.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(template);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplateExists(template.Id))
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
            return View(template);
        }

        // GET: Templates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates
                .SingleOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return PartialView(template);
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var template = await _context.Templates.SingleOrDefaultAsync(m => m.Id == id);
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemplateExists(int id)
        {
            return _context.Templates.Any(e => e.Id == id);
        }
    }
}
