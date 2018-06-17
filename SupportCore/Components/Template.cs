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
    public class Template: ViewComponent
    {
        private readonly Context _context;
        public Template(Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Event)
        {
            var templates = _context.Templates.Where(n => n.EventType == Event);
            var def =await templates.SingleOrDefaultAsync(n => n.IsDefault);
            ViewData["Templates"] = new SelectList(templates, "Id", "Name",def?.Id);
            return View();
        }
    }
}
