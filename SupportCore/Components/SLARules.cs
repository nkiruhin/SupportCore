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
    public class SLARules : ViewComponent
    {
        private readonly Context _context;
        public SLARules(Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int ParentId)
        {
            //ViewBag.TypeList = new List<SelectListItem> {
            //    new SelectListItem{ Value="text",Text="Краткий ответ" },
            //    new SelectListItem{ Value="memo",Text="Развёрнутый ответ" },
            //    new SelectListItem{ Value="date",Text="Дата и Время" },
            //    new SelectListItem{ Value="phone",Text="Номер телефона" },
            //    new SelectListItem{ Value="checkbox",Text="Флажок" },
            //    new SelectListItem{ Value="select",Text="Варианты" },
            //    new SelectListItem{ Value="file",Text="Загрузка файла" },
            //    //new SelectListItem{ Value="break",Text="Разрыв Секции" },
            //    //new SelectListItem{ Value="info",Text="Информация" }
            //};
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View(await _context.SLAs.AsNoTracking().Where(s => s.ParentId == ParentId).ToListAsync());
        }
    }
}
