﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.Models;
using SupportCore.ViewModels;
using System.Runtime.Serialization;

namespace SupportCore.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly Context _context;

        public OrganizationsController(Context context)
        {
            _context = context;
        }

        // GET: Organizations
        public async Task<IActionResult> Index()
        {
            return PartialView(await _context.Organizations.ToListAsync());
        }

        public async Task<ActionResult> List(string term,bool isProvider=false)
        {          
                var results = await _context.Organizations.AsNoTracking()
                    .Where(n => (String.IsNullOrEmpty(term)||n.Name.Contains(term))&&n.isProvider==isProvider)
                    .OrderBy(n => n.Name).Take(10)
                    .Select(r => new
                         {
                             id = r.Id,
                             text = r.Name + $"<{r.Email}>"
                          }).ToListAsync();
                return Json(new { results });
            
        }
        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return PartialView(organization);
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            ViewData["SLAId"] = new SelectList(_context.SLAs.Where(n=>n.Type==0), "Id", "Name");
            ViewData["CuratorId"] = new SelectList(_context.Person.Where(n => n.IsStaff == true), "Id", "Name");
            return PartialView();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Contract,Address,Email,Telephone,isProvider,SLAId,CuratorId")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                organization.CreateTime = DateTime.Now;
                organization.EditTime = DateTime.Now;
                organization.isDeleted = false;
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            ViewData["SLAId"] = new SelectList(_context.SLAs.Where(n => n.Type == 0), "Id", "Name",organization.SLAId);
            ViewData["CuratorId"] = new SelectList(_context.Person.Where(n => n.IsStaff == true), "Id", "Name",organization.CuratorId);
            if (organization == null)
            {
                return NotFound();
            }
            return PartialView(organization);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Contract,Address,Email,Telephone,isProvider,SLAId,CuratorId")] Organization organization)
        {
            if (id != organization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
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
            return PartialView(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return PartialView(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organization = await _context.Organizations.SingleOrDefaultAsync(m => m.Id == id);
            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
            return _context.Organizations.AsNoTracking().Any(e => e.Id == id);
        }
    }
}
