using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SupportCore.App.Classes;
using SupportCore.App.Interfaces;
using SupportCore.Models;

namespace SupportCore.Controllers
{
    public class TicketsController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IOptions<AppSetting> _appSetting;
        private readonly IHubContext<SignalrHub> _contextHub;
        public TicketsController(Context context, UserManager<User> userManager, IOptions<AppSetting> appSetting, IEmailService emailService, IHubContext<SignalrHub> contextHub)
        {
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
            _appSetting = appSetting;
            _contextHub = contextHub;
        }

        // GET: Tickets
        public IActionResult Index(Filter filter)
        {
            //ViewBag.filter = filter.ToString();
            //if (filter == null)
            //{
            //    filter = new Filter
            //    {
            //        StaffId = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User))?.Id
            //    };
            //    return PartialView(filter);
            //}
            return PartialView(filter);
        }
        // GET: Tickets for datatable in json format
        //public JsonResult IndexJson(int length,int start,int draw,int filter)
        //{
        //    //string Search = this.Request.Query["search[value]"];
        //    //int count = _context.Tickets.AsNoTracking().Count(); 
        //    string Priority = "";
        //    var StaffId = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User))?.Id;
        //    Func<Ticket,bool> filterExpresson=p=>p.StaffId==StaffId;
        //    switch (filter)
        //    {
        //        case 1://Open Ticket
        //            filterExpresson = p => p.StatusId == 1 && p.StaffId == StaffId;
        //            break;
        //        case 2://Close Tickets
        //            filterExpresson = p => p.StatusId == 2 && p.StaffId == StaffId;
        //            break;
        //        case 3://Due tickets
        //            filterExpresson = p => p.DueDate.Date<DateTime.Now.Date && p.StaffId == StaffId&&p.StatusId==1;
        //            break;
        //        case 4://Custom filter
        //            filterExpresson = GetFilter().Item1;
        //            Priority = GetFilter().Item2;
        //            break;
        //        case 5:
                    
        //            break;
        //        default:
        //            break;
        //    }
       
        //    var data =  _context.Tickets.AsNoTracking().
        //        Include(t => t.Category).
        //        Include(p => p.Person).
        //        Include(s => s.Stuff).
        //        Include(s => s.CoAuthors).
        //        Include(f=>f.Files).
        //        Include(t=>t.Tasks).
        //        Include(f=>f.FormEntryValue).
        //            ThenInclude(f=>f.Field).
        //        Where(filterExpresson).
        //        OrderByDescending(d => d.DateCreate).
        //        Select(d => new
        //        {
        //            d.Id,
        //            dateCreate = d.DateCreate.ToString("g"),
        //            d.Name,
        //            Person = d.Person?.Name,
        //            Staff = d.Stuff?.Name,
        //            Status = d.StatusId,
        //            d.SourceId,
        //            Category = d.Category.Name,
        //            IsOverdue= d.DueDate.Date<DateTime.Now.Date&&d.StatusId==1 ? true : false,
        //            IsCoAthors = d.CoAuthors.Count() == 0 ? false:true,
        //            withFiles = d.Files.Count()==0 ? false:true,
        //            withTasks = d.Tasks.Count() == 0 ? false : true,
        //            Priority = d.FormEntryValue.SingleOrDefault(e=>e.Field.Label=="priority")?.Value
        //        })/*.Skip(start).Take(length).*/
        //        .ToList();
        //        if(!String.IsNullOrEmpty(Priority)) data=data.Where(d => d.Priority == Priority).ToList();
        //        int recordsFiltered = data.Count();
        //        return Json(new {/*recordsFiltered = count, recordsTotal = count,*/ data, recordsFiltered });
        //}
        public JsonResult IndexJson(int length, int start, int draw, int filter,Filter glfilter=null)
        {
           
            //string Search = this.Request.Query["search[value]"];
            //int count = _context.Tickets.AsNoTracking().Count(); 
            Func<Ticket, bool> filterExpresson; 
            string Priority = "";
            if (String.IsNullOrEmpty(glfilter.UserId))
            {
                if(HttpContext.User.IsInRole("Пользователь")) {
                    glfilter.PersonId = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User))?.Id;
                    glfilter.isInform = true;
                } else { 
                    glfilter.StaffId = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User))?.Id;
                }
            }
            
            if (glfilter.StatusId == 100)//for due tickets
            {
                if (!String.IsNullOrEmpty(glfilter.PersonId)) {
                    filterExpresson = p => p.DueDate.Date < DateTime.Now.Date && p.PersonId == glfilter.PersonId && p.StatusId == 1 && p.IsInform;
                } else { 
                    filterExpresson = p => p.DueDate.Date < DateTime.Now.Date && p.StaffId == glfilter.StaffId && p.StatusId == 1;
                }
            } else if (glfilter.StatusId == 101)
            {
                filterExpresson = p => !p.IsAnswered&&(p.StaffId==glfilter.StaffId || String.IsNullOrEmpty(p.StaffId))&&p.StatusId == 1;
            }
            else { 
            filterExpresson = GetFilter(glfilter).Item1;
            Priority = GetFilter(glfilter).Item2;
            }
            var data = _context.Tickets.AsNoTracking().
                Include(t => t.Category).
                Include(p => p.Person).
                Include(s => s.Stuff).
                Include(s => s.CoAuthors).
                Include(f => f.Files).
                Include(t => t.Tasks).
                Include(f => f.FormEntryValue).
                    ThenInclude(f => f.Field).
                Where(filterExpresson).
                OrderByDescending(d => d.DateCreate).
                Select(d => new
                {
                    d.Id,
                    dateCreate = d.DateCreate.ToString("g"),
                    d.Name,
                    Person = d.Person?.Name,
                    Staff = d.Stuff?.Name,
                    Status = d.StatusId,
                    d.SourceId,
                    Closed = d.Closed.ToString("g"),
                    Category = d.Category.Name,
                    IsOverdue = d.DueDate.Date < DateTime.Now.Date && d.StatusId == 1 ? true : false,
                    IsCoAthors = d.CoAuthors.Count() == 0 ? false : true,
                    withFiles = d.Files.Count() == 0 ? false : true,
                    withTasks = d.Tasks.Count() == 0 ? false : true,
                    Priority = d.FormEntryValue.SingleOrDefault(e => e.Field.Label == "priority")?.Value
                })/*.Skip(start).Take(length).*/
                .ToList();
            if (!String.IsNullOrEmpty(Priority)) data = data.Where(d => d.Priority == Priority).ToList();
            int recordsFiltered = data.Count();
            return Json(new {/*recordsFiltered = count, recordsTotal = count,*/ data, recordsFiltered });
        }
        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.AsNoTracking()
                .Include(p=>p.Person)
                .Include(c=>c.Category)
                .Include(s=>s.Stuff)
                .Include(th=>th.TicketThreads)
                .Include(c=>c.CoAuthors)
                    .ThenInclude(p=>p.Person)
                .Include(t=>t.Tasks)
                    .ThenInclude(s => s.Staff)
                 .Include(t => t.Tasks)
                    .ThenInclude(o => o.Organization)
                .Include(f => f.Files)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (HttpContext.User.IsInRole("Пользователь"))
            {
                ticket.TicketThreads = ticket.TicketThreads.Where(n => n.IsInform).ToList();
            }
            ViewBag.Source = new SourceList().List.SingleOrDefault(n => n.Value == ticket.SourceId.ToString())?.Text;
            return PartialView(ticket);
        }
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.AsNoTracking()
                .Include(p => p.Person)
                .Include(c => c.Category)
                .Include(s => s.Stuff)
                .Include(th => th.TicketThreads)
                .Include(c => c.CoAuthors)
                    .ThenInclude(p => p.Person)
                .Include(t => t.Tasks)
                    .ThenInclude(o => o.Organization)
                 .Include(t => t.Tasks)
                    .ThenInclude(s => s.Staff)
                .Include(f => f.Files)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (HttpContext.User.IsInRole("Пользователь"))
            {
                ticket.TicketThreads = ticket.TicketThreads.Where(n => n.IsInform).ToList();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["SourceId"] = new SourceList().List;
            return PartialView(ticket);
        }
        // GET: Tickets/Create
        [HttpGet]
        public async Task<IActionResult> Create(uint RequestId=0,string PersonId=null)
        {
            Person Staff = null;
            Ticket ticket = new Ticket();
            if (User.IsInRole("Пользователь"))
            {
               var Person = _context.Person.AsNoTracking()
                   .Include(o=>o.Organization)
                   .SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User));
                PersonId = Person.Id;
                Staff = await _context.Person.AsNoTracking().SingleOrDefaultAsync(p => p.Id == Person.Organization.CuratorId);
                ticket.SourceId = 4;
            }
            else { 
                Staff = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User));
            }

            ticket.StaffId = Staff?.Id;
            ticket.DueDate = DateTime.Now.AddHours(24);
            if (RequestId != 0)
            {
                var email = await _emailService.ReadEmailAsync(RequestId, _context);
                ticket.SourceId = 2;
                ticket.Name = email.Subject;
                ViewBag.RequestBody = email.Text;
            }
            if (PersonId != null)
            {
                ticket.PersonId = PersonId;
                ViewBag.PersonName = _context.Person.AsNoTracking().SingleOrDefault(p => p.Id == PersonId).Name;
            }
            ViewBag.StaffName = Staff?.Name;
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["SourceId"] = new SourceList().List;
            return PartialView(ticket);
        }
        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId","DueDate","PersonId","Name","SourceId","StaffId")]Ticket ticket,List<IFormFile> Files,bool isInform)
        {
            var par = HttpContext.Request.Form.ToList();
            if (Files.Count == 0)
            {
                par.Remove(new KeyValuePair<String, Microsoft.Extensions.Primitives.StringValues>("Files", ""));
            }
            int startFormEntry = 7;
            ticket.DateCreate = ticket.DateUpdate=DateTime.Now;
            ticket.StatusId = 1; // Open status, see Event model 
            ticket.IsInform = isInform;
            bool isUser = false;
            if (HttpContext.User.IsInRole("Пользователь")) { // Set answered flag to false for users tickets
                ticket.IsAnswered = false;
                isUser = true; }
            else
            {
                ticket.IsAnswered = true;
                ticket.LastResponse = DateTime.Now;
            }
            var Staff = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User));
            var Person = _context.Person.AsNoTracking().SingleOrDefault(p => p.Id == ticket.PersonId);
            ticket.FormEntryValue = new List<FormEntryValue>();
            for (var i = startFormEntry; i < par.Count - 1; i++)
            {
                ticket.FormEntryValue.Add(new FormEntryValue
                {
                    FieldId = Int16.Parse(par[i].Key),
                    Value = par[i].Value
                });
            }
            string Inform = "";
            if (isInform)
            {
                Inform = "Произведено информирование пользователя";
            }
            ticket.TicketThreads = new List<TicketThread>
            {
                new TicketThread
                {
                    DateCreate = DateTime.Now,
                    Title = "Заявка создана",
                    Poster = Staff.Name,
                    PersonId = Staff.Id,
                    Body = $"Создана заявка. {Inform}",
                    Type = 0,
                    IsInform = isInform
                }
            };
            try
            {
                if (Files.Count > 0)
                {
                    ticket.Files = await UploadFilesAsync(Files);
                }
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                string[] ExceptCon = SignalrHub._connections.GetConnections(_userManager.GetUserId(HttpContext.User)).Cast<string>().ToArray();
                await _contextHub.Clients.GroupExcept("Staff", ExceptCon).SendAsync("ReceiveMessage", Staff.Name, DateTime.Now.ToString("g"), $"<a href=\"/Tickets/Edit/{ticket.Id}\" data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-update=\"#cell-content\" data-ajax-mode=\"update\" title=\"Перейти\">Заявка #{ticket.Id}</a>: Добавлена новая заявка",isUser);
                if (isInform)
                {
                    string message = $"По вашему обращению открыта заявка #{ticket.Id}. Благодарим за обращение";
                    var template = await _context.Templates.FirstOrDefaultAsync(t => t.EventType == 1 && t.IsDefault == true);
                    if (template != null)
                    {
                        message = await new Templates(_context).GetTemplate(template, ticket);
                    }
                    await _emailService.SendEmailAsync(Person.Email, $"Создана заявка #{ticket.Id}", message);
                }
               
                return RedirectToAction(nameof(Edit), new { id = ticket.Id });
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError(string.Empty, "Ошибка при сохранении:" + ex.Message);
            }
            ViewBag.StaffName = Staff.Name;
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["SourceId"] = new SourceList().List;
            return PartialView(ticket);
        }
        // POST: Tickets/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, List<IFormFile> Files)
        {
            if (id == null)
            {
                return NotFound();
            }
            var par = HttpContext.Request.Form.ToList();
            if (Files.Count == 0)//Delete files input from colection
            {
                par.Remove(new KeyValuePair<String, Microsoft.Extensions.Primitives.StringValues>("Files", ""));
            }
            int startFormEntry = 6;
            if (HttpContext.User.IsInRole("Пользователь")) startFormEntry = 3;
            Ticket ticketforUpdate = await _context.Tickets
                .Include(fe=>fe.FormEntryValue)
                .SingleOrDefaultAsync(t => t.Id == id);
            ticketforUpdate.DateUpdate = DateTime.Now;
            bool result=await TryUpdateModelAsync<Ticket>(ticketforUpdate);
            ticketforUpdate.FormEntryValue.Clear();
            for (var i = startFormEntry; i < par.Count - 1; i++)
            {
                    ticketforUpdate.FormEntryValue.Add(new FormEntryValue
                    {
                        FieldId = Int16.Parse(par[i].Key),
                        Value = par[i].Value
                    });
            }
            try
            {
                if (Files.Count > 0)
                {
                    ticketforUpdate.Files = await UploadFilesAsync(Files);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit));
            }
            catch (DbUpdateException ex)
            {
                //    //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError(String.Empty, "Ошибка при сохранении. " + ex.Message);
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["SourceId"] = new SourceList().List;
            return PartialView();
        }

        // Function for expression filter 
        private Tuple<Func<Ticket, bool>,string> GetFilter(Filter filter=null)
        {
            if(filter==null) filter = _context.Filters.SingleOrDefault(n => n.UserId == _userManager.GetUserId(HttpContext.User));
            bool exp(Ticket p) =>
                 (String.IsNullOrEmpty(filter.PersonId) || p.PersonId == filter.PersonId) &&
                 (String.IsNullOrEmpty(filter.StaffId) || p.StaffId == filter.StaffId) &&
                 (filter.DateCreate1 == null || p.DateCreate >= filter.DateCreate1) &&
                 (filter.DateCreate2 == null || p.DateCreate <= filter.DateCreate2) &&
                 (filter.SourceId == 0 || p.SourceId == filter.SourceId) &&
                 (filter.StatusId == 0 || p.StatusId == filter.StatusId) &&
                 (filter.CategoryId == 0 || p.CategoryId == filter.CategoryId) &&
                 (p.IsInform == filter.isInform || p.IsInform);
            return new Tuple<Func<Ticket, bool>, string>(exp, filter.Priority);
        }

        // GET: Tickets/Filter/
        public IActionResult Filter(int? isWin)
        {
            var UserId = _userManager.GetUserId(HttpContext.User);
            var filter = _context.Filters.AsNoTracking().SingleOrDefault(f => f.UserId == UserId);
            if (filter != null) { 
            ViewBag.PersonName = _context.Person.AsNoTracking().SingleOrDefault(p => p.Id == filter.PersonId)?.Name;
            ViewBag.StaffName = _context.Person.AsNoTracking().SingleOrDefault(p => p.Id == filter.StaffId)?.Name;
            }
            if (isWin == 1) ViewBag.isWin = "style=max-width:1000px";
            ViewData["StatusId"] = new Event().StatusList();
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["SourceId"] = new SourceList().List;
            ViewData["priority"] = _context.Fields.AsNoTracking().SingleOrDefault(n => n.Label == "priority").Configuration;
            return PartialView(filter);
        }
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Filter(int? id,Filter filter)
        {
            bool result;
            if (id == null)
            {
                Filter f = new Filter
                {
                    UserId = _userManager.GetUserId(HttpContext.User),
                };
                result = await TryUpdateModelAsync<Filter>(f);
                await _context.Filters.AddAsync(f);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", filter);
                //return Ok("ОK");
            }
            filter.UserId = _userManager.GetUserId(HttpContext.User);
            _context.Filters.Update(filter);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", filter);
            //return Ok("ОK"); 
        }
        // Get Tickets/GetTicketCounters
        public JsonResult GetTicketCounters()
        {
            if (HttpContext.User.IsInRole("Пользователь"))
            {
                var PersonId = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User))?.Id;
                Counters count = new Counters
                {
                    overlayTicketCount = _context.Tickets.AsNoTracking().Where(p => p.DueDate.Date < DateTime.Now.Date && p.PersonId == PersonId && p.StatusId == 1&&p.IsInform).Count(),
                    openTicketCount = _context.Tickets.AsNoTracking().Where(p => p.StatusId == 1 && p.PersonId == PersonId&&p.IsInform).Count(),
                    closeTicketCount = _context.Tickets.AsNoTracking().Where(p => p.StatusId == 2 && p.PersonId == PersonId&&p.IsInform).Count(),
                    myTicketCount = _context.Tickets.AsNoTracking().Where(p => p.PersonId == PersonId&& p.IsInform).Count()                 
                };
                return Json(count);
                }
                else { 
                var StaffId = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User))?.Id;
                Counters count = new Counters {
                    overlayTicketCount = _context.Tickets.AsNoTracking().Where(p => p.DueDate.Date < DateTime.Now.Date && p.StaffId == StaffId && p.StatusId == 1).Count(),
                    openTicketCount = _context.Tickets.AsNoTracking().Where(p => p.StatusId == 1 && p.StaffId == StaffId).Count(),
                    closeTicketCount = _context.Tickets.AsNoTracking().Where(p => p.StatusId == 2 && p.StaffId == StaffId).Count(),
                    myTicketCount = _context.Tickets.AsNoTracking().Where(p => p.StaffId == StaffId).Count(),
                    noanswerTicketCount = _context.Tickets.AsNoTracking().Where(p => !p.IsAnswered && (p.StaffId == StaffId || String.IsNullOrEmpty(p.StaffId)) && p.StatusId == 1).Count(),
                    ticketThreadsCount = _context.TicketThreads.AsNoTracking().OrderByDescending(n => n.DateCreate).Where(n => n.DateCreate.Date == DateTime.Now.Date).Count()
                };
                return Json(count);
            }
            
        }
        // Get: Tickets/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
            return PartialView(ticket);
        }
        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets
                .Include(f=>f.Files)
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
        public IActionResult GetCustomForm(int CategoryId)
        {
            var form = _context.Category.AsNoTracking().SingleOrDefault(c => c.Id == CategoryId).FormId;
            if (form != null) { 
            return ViewComponent("Form",new { _context.Category.AsNoTracking().SingleOrDefault(c => c.Id == CategoryId).FormId });
            }
            return new EmptyResult();
        }
        //POST: Tickets/AddCoAuthors/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCoAuthors(int Id, string[] coAuthors,string Name)
        {
            string[] coAuthorsOld = await _context.CoAuthors
                .AsNoTracking()
                .Where(n => n.TicketId == Id).Select(n => n.PersonId).ToArrayAsync<string>();
            if (coAuthors.Length == 0&&coAuthorsOld.Length==0) return BadRequest("Нет соавторов");           
            string [] coAuthorsforAdd=coAuthors.Except(coAuthorsOld).ToArray();
            string[] coAuthorsforRemove = coAuthorsOld.Except(coAuthors).ToArray();
            if(coAuthorsforAdd.Length==0&&coAuthorsforRemove.Length==0) return BadRequest("Нет соавторов");
            string coAuthorsList="";
            if (coAuthorsforAdd.Length > 0)// Add new coauthors
            {
                List<CoAuthor> coAuthorforAdd = new List<CoAuthor>();
                for (int i = 0; i < coAuthorsforAdd.Length; i++)
                {
                    var person = _context.Person.AsNoTracking().SingleOrDefault(n => n.Id == coAuthorsforAdd[i]);

                    //await _emailService.SendEmailAsync(person.Email, $"Вы добавлены сооавтором заявки #{Id}",
                    //    $"<p>Уважаемый пользователь!</p><p>Вы добавлены в качестве соавтора к заявке: {Name}</p><p>Номер заявки: #{Id} <br><br>");
                    coAuthorforAdd.Add(new CoAuthor
                    {
                        TicketId = Id,
                        PersonId = person.Id,
                        Email = person.Email
                    });
                    coAuthorsList = coAuthorsList + $" {person.Name}<{person.Email}>";
                }

                await _context.CoAuthors.AddRangeAsync(coAuthorforAdd);
                string message = $"<p>Уважаемый пользователь!</p><p>Вы добавлены в качестве соавтора к заявке: {Name}</p><p>Номер заявки: #{Id} <br><br>";
                var template = await _context.Templates.FirstOrDefaultAsync(t => t.EventType == 6 && t.IsDefault == true);
                if (template != null)
                {
                    message = await new Templates(_context).GetTemplate(template, Id);
                }
                await _emailService.SendEmailAsync("", "Вы добавлены сооавтором заявки", message, coAuthorforAdd);
            }
            if (coAuthorsforRemove.Length > 0)// Remove coauthors
            { 
                //List<CoAuthor> coAuthorforRemove = new List<CoAuthor>();
                for (int i = 0; i < coAuthorsforRemove.Length; i++)
                {
                    var coAuthorforRemove = _context.CoAuthors.SingleOrDefault(n => n.TicketId == Id && n.PersonId == coAuthorsforRemove[i]);
                    _context.CoAuthors.Remove(coAuthorforRemove);
                }
            }
            var Person = await _context.Person.AsNoTracking().Select(t => new { t.Id, t.Name, t.AccountID }).SingleOrDefaultAsync(p => p.AccountID == _userManager.GetUserId(HttpContext.User));
            await _context.TicketThreads.AddAsync(new TicketThread {
                TicketId=Id,
                Title="Добавлены сооавторы",
                DateCreate=DateTime.Now,
                Body=$"Добавлены сооавторы:{coAuthorsList}",
                Poster= Person.Name,
                PersonId=Person.Id,
                Type=0
            });
            await _context.SaveChangesAsync();
            string[] ExceptCon = SignalrHub._connections.GetConnections(_userManager.GetUserId(HttpContext.User)).Cast<string>().ToArray();
            await _contextHub.Clients.AllExcept(ExceptCon).SendAsync("ReceiveMessage", Person.Name,DateTime.Now.ToString("g"), $"Заявка #{Id}: Добавлены сооавторы");
            return RedirectToAction("Index", "TicketThreads", new { Id });
        }
        public async Task<List<Models.File>> UploadFilesAsync(List<IFormFile> Files)
        {
            if (Files.Sum(f => f.Length)> 10485760)
            {
                return null;
            }
            //http://www.mikesdotnetting.com/article/288/asp-net-5-uploading-files-with-asp-net-mvc-6
            //http://dotnetthoughts.net/file-upload-in-asp-net-5-and-mvc-6/
            //https://docs.microsoft.com/ru-ru/aspnet/core/mvc/models/file-uploads?view=aspnetcore-2.0
            List<Models.File> TicketFiles = new List<Models.File>();
            foreach (var formFile in Files)
            {
                if (formFile.Length > 0)
                {
                    var name = formFile.FileName;
                    var path = Path.Combine(_appSetting.Value.UploadDirectory, name);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                     TicketFiles.Add(new Models.File
                    {
                        FileName = name,
                        CreatedTimestamp = DateTime.Now,
                        UpdatedTimestamp = DateTime.Now,
                        ContentType = formFile.ContentType
                    });
                }
            }
            return TicketFiles;
        }
        public async Task<IActionResult> Download(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            
            var fileforDownload = await _context.Files.AsNoTracking().SingleOrDefaultAsync(f => f.Id == id);
            var path = Path.Combine(_appSetting.Value.UploadDirectory, fileforDownload.FileName);
            var stream = new FileStream(path, FileMode.Open);
            return File(stream, fileforDownload.ContentType, fileforDownload.FileName);
        }
        public async Task<IActionResult> getDescription(int? TicketId)
        {
            if (TicketId == null)
            {
                return NotFound();
            }
            var desc = await _context.FormEntryValues.AsNoTracking()
                .Include(f=>f.Field)
                .SingleOrDefaultAsync(n => n.TicketId == TicketId && n.Field.Label == "description");
            if(desc!=null) { 
            return Ok(desc.Value);
            }
            return NotFound();
        }
        //POST Tickets/Send
        //public async Task<IActionResult> Send()
        //{
        //    var client = new SignalRclient();
        //    client.SendEventAsync(1);
        //    return Ok("Послали");
        //}
        // POST: Tickets/EntrySystemValues/
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EntrySystemValues(int TicketId)
        //{
        //    var formEntry = new FormEntryValue();
        //    var par = HttpContext.Request.Form.ToList();   
        //    for (int i = 1; i < par.Count-2; i++)
        //    {
        //        formEntry.TicketId = TicketId;
        //        formEntry.FieldId = Int32.Parse(par[i].Key);
        //        formEntry.Value = par[i].Value;
        //        _context.Add(formEntry);
        //        await _context.SaveChangesAsync();
        //        formEntry.Id = null;
        //    }
        //    return Ok("ок");
        //}
    }
}
