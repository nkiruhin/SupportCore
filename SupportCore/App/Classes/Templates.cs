using Microsoft.EntityFrameworkCore;
using SupportCore.Models;
using SupportCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Templates 
{
        private readonly Context _context;

    public Templates()
    {
    }

    public Templates(Context context)
        {
             _context = context;
        }
    public async Task<string> GetTemplate(int? id, int TicketId)
        {
            if (id == null)
            {
                return "empty";
            }
            var template = await _context.Templates.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return "empty";
            }
            //var engine = new RazorLight.RazorLightEngineBuilder().UseMemoryCachingProvider()
            //  .Build();
            var model = await _context.Tickets.AsNoTracking()
                .Include(n => n.Person)
                .Include(n => n.Stuff)
                .SingleOrDefaultAsync(n => n.Id == TicketId);
            var defaultThread = _context.FormEntryValues.AsNoTracking().Where(n => n.Field.Form.Type == 0 && n.TicketId == TicketId);
        string result = template.Body
        .Replace("@ДатаСоздания", model.DateCreate.ToString())
        .Replace("@Пользователь", model.Person.Name)
        .Replace("@Номер", model.Id.ToString())
        .Replace("@Тема", model.Name)
        .Replace("@ПлановаяДата", model.DueDate.ToShortDateString())
        .Replace("@ДатаЗакрытия", model.Closed.Date.ToString())
        .Replace("@Сотрудник", model.Stuff?.Name);
        //Wait when support net cor 2.1 in RazorLith
            //string result = await engine.CompileRenderAsync(template.Id.ToString(), template.Body.Replace("@", "@Model."), new TemplateView
            //{
            //    ДатаСоздания = model.DateCreate,
            //    Пользователь = model.Person.Name,
            //    Номер = model.Id,
            //    Тема = model.Name,
            //    ПлановаяДата = model.DueDate.ToShortDateString(),
            //    ДатаЗакрытия = model.Closed,
            //    Сотрудник = model.Stuff.Name
            //});
            return result;
        }

    public async Task<string> GetTemplate(Template template, int TicketId)
    {
        //var engine = new RazorLight.RazorLightEngineBuilder().UseMemoryCachingProvider()
        //  .Build();
        var ticket = await _context.Tickets.AsNoTracking()
                .Include(n => n.Person)
                .Include(n => n.Stuff)
                .SingleOrDefaultAsync(n => n.Id == TicketId);
        var defaultThread = _context.FormEntryValues.AsNoTracking().Where(n => n.Field.Form.Type == 0 && n.TicketId == ticket.Id);
        if (ticket.Person == null)
        {
            ticket.Person = _context.Person.AsNoTracking().SingleOrDefault(n => n.Id == ticket.PersonId);
        }
        if (ticket.Stuff == null)
        {
            ticket.Stuff = await _context.Person.AsNoTracking().SingleOrDefaultAsync(n => n.Id == ticket.StaffId);
        }
        string result = template.Body
       .Replace("@ДатаСоздания", ticket.DateCreate.ToString())
       .Replace("@Пользователь", ticket.Person.Name)
       .Replace("@Номер", ticket.Id.ToString())
       .Replace("@Тема", ticket.Name)
       .Replace("@ПлановаяДата", ticket.DueDate.ToShortDateString())
       .Replace("@ДатаЗакрытия", ticket.Closed.Date.ToString())
       .Replace("@Сотрудник", ticket.Stuff.Name);
        //string result = await engine.CompileRenderAsync(template.Id.ToString(), template.Body.Replace("@", "@Model."), new TemplateView
        //{
        //    ДатаСоздания = ticket.DateCreate,
        //    Пользователь = ticket.Person.Name,
        //    Номер = ticket.Id,
        //    Тема = ticket.Name,
        //    ПлановаяДата = ticket.DueDate.ToShortDateString(),
        //    ДатаЗакрытия = ticket.Closed,
        //    Сотрудник = ticket.Stuff.Name
        //});
        return result;
    }
    public async Task<string> GetTemplate(Template template, Ticket ticket)
    {
        //var engine = new RazorLight.RazorLightEngineBuilder().UseMemoryCachingProvider()
        //  .Build();
        var defaultThread = _context.FormEntryValues.AsNoTracking().Where(n => n.Field.Form.Type == 0 && n.TicketId == ticket.Id);
        if (ticket.Person == null)
        {
            ticket.Person = _context.Person.AsNoTracking().SingleOrDefault(n => n.Id == ticket.PersonId);
        }
        if (ticket.Stuff == null)
        {
            ticket.Stuff = await _context.Person.AsNoTracking().SingleOrDefaultAsync(n => n.Id == ticket.StaffId);
        }
        string result = template.Body
       .Replace("@ДатаСоздания", ticket.DateCreate.ToString())
       .Replace("@Пользователь", ticket.Person.Name)
       .Replace("@Номер", ticket.Id.ToString())
       .Replace("@Тема", ticket.Name)
       .Replace("@ПлановаяДата", ticket.DueDate.ToShortDateString())
       .Replace("@ДатаЗакрытия", ticket.Closed.Date.ToString())
       .Replace("@Сотрудник", ticket.Stuff?.Name);
        //string result = await engine.CompileRenderAsync(template.Id.ToString(), template.Body.Replace("@", "@Model."), new TemplateView
        //{
        //    ДатаСоздания = ticket.DateCreate,
        //    Пользователь = ticket.Person.Name,
        //    Номер = ticket.Id,
        //    Тема = ticket.Name,
        //    ПлановаяДата = ticket.DueDate.ToShortDateString(),
        //    ДатаЗакрытия = ticket.Closed,
        //    Сотрудник = ticket.Stuff.Name
        //});
        return result;
    }
}
