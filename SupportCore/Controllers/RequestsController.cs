using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.App.Interfaces;
using SupportCore.Models;

namespace SupportCore.Controllers
{
    public class RequestsController : Controller
    {
        private readonly Context _context;
        private readonly IEmailService _emailService;
        public RequestsController(Context context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            return PartialView(await _emailService.GetEmailsAsync(_context) );
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(uint id)
        {
            var request = await _emailService.ReadEmailAsync(id, _context);
            return PartialView(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateCreate,DateUpdate,Email,From,IsDeleted,PersonId,Phone,Subject,Text,TicketId")] Requests requests)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requests);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests.SingleOrDefaultAsync(m => m.Id == id);
            if (requests == null)
            {
                return NotFound();
            }
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,DateCreate,DateUpdate,Email,From,IsDeleted,PersonId,Phone,Subject,Text,TicketId")] Requests requests)
        {
            if (id != requests.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestsExists(requests.Id))
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
            return View(requests);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .SingleOrDefaultAsync(m => m.Id == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // POST: Requests/Delete/5 Make read message inbox!
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            _emailService.MakeReadAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Requests/Attach/uid Make read message inbox!
        public async Task<IActionResult> Attach(uint id,string FileName)
        {
            //_emailService.MakeReadAsync(id);
            
            var stream=await _emailService.GetAttach(id, FileName);
            return File(stream.Stream, stream.ContentType);
            //return RedirectToAction(nameof(Index));
        }

        private bool RequestsExists(string id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
