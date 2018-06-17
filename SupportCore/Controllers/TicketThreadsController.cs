﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.App.Interfaces;
using SupportCore.Models;

namespace SupportCore.Controllers
{
    public class TicketThreadsController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        public TicketThreadsController(Context context, UserManager<User> userManager,IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: TicketThreads
        public async Task<IActionResult> Index(int Id)
        {
            var context = _context.TicketThreads.AsNoTracking().Where(n=>n.TicketId==Id).OrderByDescending(n=>n.DateCreate);
            return PartialView(await context.ToListAsync());
        }

        // GET: TicketThreads/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketThread = await _context.TicketThreads.AsNoTracking()
                .Include(t => t.Person)
                .Include(t => t.Ticket)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ticketThread == null)
            {
                return NotFound();
            }

            return PartialView(ticketThread);
        }

        // GET: TicketThreads/Create/Id
        public IActionResult Create(int Id, int Event, int Type,string table)
        {
            if (Event == 2&&_context.Tasks.AsNoTracking().Where(t => t.TicketId == Id && t.Status == 0).Count()>0)
            {
                //return new HtmlString("Действие невозможно!В заявке имеются не закрытые задачи!");
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<div class='p-3'><h6><p>Действие невозможно!В заявке имеются не закрытые задачи!</h6></div>"
                };
            }
            if (table == null) table="false" ;
            ViewBag.Event = Event;
            ViewBag.table = table;
            var thread = new TicketThread() { TicketId = Id, Type=Type};
            return PartialView(thread);
        }

        // POST: TicketThreads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Body,TicketId")] TicketThread ticketThread,int Event,bool isInform,bool table)
        {
            var events = new Event();
            ticketThread.Title = events.GetEvent(Event);
            if (ModelState.IsValid)
            {
                var Person = await _context.Person.AsNoTracking().Select(t => new { t.Id, t.Name, t.AccountID }).SingleOrDefaultAsync(p => p.AccountID == _userManager.GetUserId(HttpContext.User));
                var ticket = await _context.Tickets
                    .Include(t => t.TicketThreads)
                    .Include(t => t.Person)
                    .Include(t=>t.CoAuthors)
                    .SingleOrDefaultAsync(t => t.Id == ticketThread.TicketId);
                if (events.IsStatus(Event)){
                    ticket.StatusId = Event;
                }
                ticket.DateUpdate = ticketThread.DateCreate = DateTime.Now;
                if (Event == 2) { ticket.Closed = DateTime.Now; }
                ticketThread.PersonId = Person.Id;
                ticketThread.Poster = Person.Name;
                _context.TicketThreads.Add(ticketThread);
                await _context.SaveChangesAsync();
                if (isInform)
                {
                    ticketThread.Title = $"Изменения в заявке #{ticket.Id} {ticketThread.Title}";
                    await _emailService.SendEmailAsync(ticket.Person.Email, ticketThread.Title, ticketThread.Body,ticket.CoAuthors);
                }
                if (table||Event == 4) return Ok("OK");
                if(Event==2) return RedirectToAction("Index","Tickets"); //If Event = close load ticket list
                return RedirectToAction("Edit","Tickets",new { Id = ticketThread.TicketId });
            }
            if (table) return new StatusCodeResult(500);
            return PartialView(ticketThread);
        }

        // GET: TicketThreads/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketThread = await _context.TicketThreads.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (ticketThread == null)
            {
                return NotFound();
            }
            //ViewData["PersonId"] = new SelectList(_context.Person, "Id", "Id", ticketThread.PersonId);
            //ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id", ticketThread.TicketId);
            return PartialView(ticketThread);
        }

        // POST: TicketThreads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirm(string id,int TicketId)
        {
           
            if (ModelState.IsValid)
            {
                var TTforUpdate = await _context.TicketThreads.SingleOrDefaultAsync(t => t.Id == id);
                TTforUpdate.DateUpdate = DateTime.Now;
                if (await TryUpdateModelAsync<TicketThread>(TTforUpdate, "", tt => tt.Body)) { 
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                  
                }
                return RedirectToAction(nameof(Index),new { id=TicketId });
                }
            }
            return StatusCode(505);
        }

        // GET: TicketThreads/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketThread = await _context.TicketThreads.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (ticketThread == null)
            {
                return NotFound();
            }

            return PartialView(ticketThread);
        }

        // POST: TicketThreads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id,int TicketId)
        {
            var ticketThread = await _context.TicketThreads.SingleOrDefaultAsync(m => m.Id == id);
            _context.TicketThreads.Remove(ticketThread);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),new { Id=TicketId });
        }

        private bool TicketThreadExists(string id)
        {
            return _context.TicketThreads.Any(e => e.Id == id);
        }
    }
}
