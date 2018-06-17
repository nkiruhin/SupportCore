using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Components
{
    [ViewComponent]
    public class Form : ViewComponent
    {
        private readonly Context _context;
        public Form(Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int FormId,int TicketId,bool Edit,string description)
        {
            if (FormId == 0) { FormId = _context.Forms.AsNoTracking().SingleOrDefault(n => n.Type == 0).Id; }
            if (TicketId==0) {
                var fl = await _context.Fields.AsNoTracking().Where(f => f.FormId == FormId).ToListAsync();
                if (!string.IsNullOrEmpty(description))
                {
                    var fd = fl.SingleOrDefault(l => l.Label == "description");
                    fd.FormEntryValue = new List<FormEntryValue>
                    {
                        new FormEntryValue
                        {
                            Value = description
                        }
                    };
                }
                return View(fl);

            }
            //var field = _context.Fields.Include(v => v.FormEntryValue.Where(t=>t.TicketId==TicketId))
            //    .Where(f => f.FormId == FormId); Look this https://github.com/aspnet/EntityFrameworkCore/issues/1833
            var fields = await _context.Fields.AsNoTracking().Where(f => f.FormId == FormId).ToListAsync();
            var files = await _context.Files.Where(n => n.TicketId == TicketId).ToArrayAsync();
            if (files != null)
            {
                foreach(var f in files) {
                    
                     ViewBag.Files = ViewBag.Files + $"&nbsp; <span class=\"mif-attachment\"></span><a href=\"Tickets\\Download\\" +f.Id+"\" target=\"_blank\">" + f.FileName+"</a>";
            }
            }
            foreach (Field f in fields)
            {
                f.FormEntryValue = await _context.FormEntryValues.AsNoTracking().Where(v => v.TicketId == TicketId & v.FieldId == f.Id).ToListAsync();
            }
            if (!Edit) return View("Detail", fields);
            return View(fields);
        }
    }
}
