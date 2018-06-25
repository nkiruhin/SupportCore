using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.Models;

namespace SupportCore.Controllers
{
    public class PersonController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public PersonController(Context context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Person
        public IActionResult Index()
        {
            //var context = _context.Person.Include(p => p.Organization);
            return PartialView();
        }
        // GET: Person
        public async Task<JsonResult> List(bool isStaff,int? OrganizationId)  {
            var data = await _context.Person
                .Include(p => p.Organization)
                .AsNoTracking()
                .Where(p => p.IsStaff == isStaff
                &&(p.OrganizationId==OrganizationId||OrganizationId==null))
                .Select(d=>new {
                    id=d.Id,
                    name =d.Name,
                    email=d.Email,
                    organization=d.Organization.Name
                }).
                ToListAsync();
            return Json(new { data });
        }
        // GET: Create Person from dialog
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }
        // GET: Create Person from dialog
        public IActionResult CreateDialog()
        {
            return PartialView();
        }
        // GET: Person/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.Organization)
                .Include(f => f.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return PartialView(person);
        }

        // GET: Person/Phone
        public IActionResult Phone(string id, string name)
        {
            //    //var Phones = _context.Phone.Where(m => m.PersonId == id);
            ViewData["id"] = id;
            ViewData["name"] = name;
            return PartialView(new Phone());
        }
        // POST: Person/AddPhone
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhone(string PersonId, Phone phone)
        {
            if (ModelState.IsValid)
            {
                phone.PersonId = PersonId;
                _context.Add(phone);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("ListPhones", new { id = PersonId });
        }
        public IActionResult ListPhones(string id)
        {
            var Phones = _context.Phone.AsNoTracking().Where(m => m.Person.Id == id);
            return PartialView(Phones);
        }
        [HttpPost]
        public async Task<IActionResult> PhoneDelete(string id, string PersonId)
        {
            var phone = await _context.Phone.SingleOrDefaultAsync(m => m.Id == id);
            _context.Phone.Remove(phone);
            await _context.SaveChangesAsync();
            return RedirectToAction("ListPhones", new { id = PersonId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PhoneSave(Phone phone)
        {
            phone.PhoneNumber = Request.Form["item.PhoneNumber"];
            _context.Update(phone);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted,AccountID,IsStaff,Email,OrganizationId")] Person person, int step)
        {
            //switch (step)
            //{

            //   case 2: return RedirectToAction();
            //   case 3:
            //   default:
            //    break;
            //}
            if (ModelState.IsValid)
            {
                person.DateCreate = person.DateUpdate = DateTime.Now;
                _context.Add(person);
                await _context.SaveChangesAsync();
                //return PartialView("Phone",person);
                return RedirectToAction("Phone", new { id = person.Id, name = person.Name });
            }

            return PartialView(person);
        }
        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FastCreate([Bind("Name,Email,OrganizationId")] Person person, string PhoneList)
        {
            if (ModelState.IsValid)
            {
               person.DateCreate = person.DateUpdate = DateTime.Now;
                if (!String.IsNullOrEmpty(PhoneList)) { 
                    string[] Phones = PhoneList.Split("\r\n");
                    person.Phones = new List<Phone>();
                    for (int i = 0; i < Phones.Length; i++)
                    {
                        person.Phones.Add(new Phone
                        {
                            PhoneNumber = Phones[i]
                        });
                    }
                }
                _context.Add(person);
                await _context.SaveChangesAsync();
                return Json(new { Id=person.Id,Text=person.Name });
            }

            return StatusCode(500);
        }
        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.AsNoTracking().
                Include(p => p.Phones).
                Include(o => o.Organization).
                SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return PartialView(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,IsDeleted,AccountID,IsStaff,Email,OrganizationId,ApiKey")] Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    person.Id = id;
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return PartialView(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return PartialView(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var person = _context.Person.Include(p => p.Phones).FirstOrDefault(m => m.Id == id);
            var phone = _context.Phone.ToList();
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            if (person.AccountID != null)
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
        // GET: Person/GetPersonForPhone/id
        public async Task<IActionResult> GetPersonForPhone(string id)
        {
            var person = await _context.Phone.AsNoTracking()
                .Include(p => p.Person)
                .FirstOrDefaultAsync(n => n.PhoneNumber == id);
            if (person != null)
            {
                return Ok(String.Format("Звонок входящий от: <a href=\"/Person/Details/{0}\" data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-update=\"#cell-content\" data-ajax-mode=\"update\" title=\"Просмотреть\">{1}<{2}></a>", person.PersonId, person.Person.Name, id));
            }
            return Ok("Звонок входящий от: Неизвестный <br>Номер: " + id);
        }
        // GET: Person/SelectList
        public async Task<JsonResult> SelectList(bool isStaff, string term)
        {
            if (term != null)
            {
                var results = await _context.Person.AsNoTracking().Where(n => n.Name.Contains(term) && n.IsStaff == isStaff).Select(r => new
                {
                    id = r.Id,
                    text = r.Name + $"<{r.Email}>"
                }).Take(10).ToListAsync();
                return Json(new { results });
            }
            else
            {
                var results = await _context.Person.AsNoTracking().Where(n => n.IsStaff == isStaff).Select(r => new
                {
                    id = r.Id,
                    text = r.Name + $"<{r.Email}>"
                }).Take(10).ToListAsync();
                return Json(new { results });
            }
        }


        public async Task<JsonResult> TicketByCategory(string id){
            var data = await _context.Tickets.AsNoTracking()
                 .Include(c=>c.Category)
                 .Where(t => t.PersonId == id)
                 .GroupBy(p => new { p.Category.Name })
                 .Select(d => new
                 {
                     labels = d.Key.Name,
                     summ = d.Count()
                 }).ToListAsync();
            return Json(new { data, type = "C" });
        }
        public async Task<JsonResult> TicketByStatus(string id)
        {
            var data = await _context.Tickets.AsNoTracking()
                 .Where(t => t.PersonId == id)
                 .GroupBy(s => new { s.StatusId })
                 .Select(d => new
                 {
                     labels = new Event().GetEvent(d.Key.StatusId),
                     summ = d.Count()
                 }).ToListAsync();
            return Json(new { data, type = "S" });
        }
        public async Task<JsonResult> TicketStaff(string id,int period)
        {
            period = -1 * period;
            var dataPeriod = DateTime.Now.AddDays(-7);
            var Tickets = await _context.Tickets.AsNoTracking()
                 .Include(th=>th.TicketThreads)
                 .Include(ts=>ts.Tasks)
                 .Where(t => t.StaffId == id & t.DateCreate > DateTime.Now.AddDays(period))
                 .ToListAsync();
            int TicketClose = Tickets.Where(c=>c.Closed> DateTime.Now.AddDays(period)).Count();
            int TicketOpen = Tickets.Count();
            //int timeAnswer = Tickets.Sum(o => ((int)o.TicketThreads.Skip(1).First()?.DateCreate.Minute)-o.DateCreate.Minute) / Tickets.Count;
            int OpenTask = Tickets.Sum(t => t.Tasks.Count());
            int TicketDue = Tickets.Where(c => c.Closed > c.DueDate).Count();
            var data = new  {
                labels = new string[] { "Открыто", "Закрыто", "Просрочено","Открыто задач" },
                series = new int[][] { new int[] { TicketOpen, TicketClose, TicketDue, OpenTask} }
            };
            return Json(data);
        }
        public async Task<Person> GetCurrentUserAsync()
        {
            return await _context.Person.AsNoTracking().SingleOrDefaultAsync(p => p.AccountID == _userManager.GetUserId(HttpContext.User));
        }
    }
}
