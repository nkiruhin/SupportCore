using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class Event
    {
        private class EventItem
        {
           public int Id { set; get; }
           public string Value { set; get; }
           public bool IsStatus { set; get; }
        }
    private List<EventItem> Events = new List<EventItem>{
        new EventItem { Id = 0, Value = "Добавлен комментарий",IsStatus=false },
        new EventItem { Id = 1, Value = "Заявка открыта" ,IsStatus=true},
        new EventItem { Id = 2, Value = "Заявка закрыта" ,IsStatus=true },
        new EventItem { Id = 3, Value = "Заявка отложена" ,IsStatus=true },
        new EventItem { Id = 4, Value = "Заявка изменена" ,IsStatus=false },
        new EventItem { Id = 5, Value = "Заявка выполнена" ,IsStatus=true },
        new EventItem { Id = 6, Value = "Добавлен соавтор",IsStatus=false  },
        new EventItem { Id = 7, Value = "Создано задание на исполнителя",IsStatus=false  },
     };
    public string GetEvent(int Event)
     {
            return Events.SingleOrDefault(n => n.Id == Event).Value;
     }
    public SelectList EventList()
        {
            return new SelectList(Events, "Id", "Value");
        }
    public SelectList StatusList()
    {
            return new SelectList(Events.Where(n => n.IsStatus == true), "Id", "Value");
    }
        public bool IsStatus(int Event)
        {
            return Events.SingleOrDefault(n => n.Id == Event).IsStatus;
        }
    }
}
