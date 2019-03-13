using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportCore.Models
{
    public class Ticket
    {
        class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "dd/MM/yyyy";
            }
        }
        public int Id { set; get; }
        [DisplayName("Тема")]
        [Required(ErrorMessage = "Поле обязательно")]
        public string Name { set; get; }
        public int StatusId { set; get; }
        [DisplayName("Источник заявки")]
        [Required(ErrorMessage = "Поле обязательно")]
        public int SourceId { set; get; }
        public int Flags { set; get; }
        public bool IsOverdue { set; get; }
        public bool IsAnswered { set; get; }
        public bool IsInform { set; get; }
        [DisplayName("Плановая дата")]
        [Required(ErrorMessage = "Поле обязательно")]
        [DataType(DataType.Date)]
        public DateTime DueDate { set; get; }
        public DateTime Reopened { set; get; }
        public DateTime Closed { set; get; }
        public DateTime LastMessage { set; get; }
        public DateTime LastResponse { set; get; }
        [DataType(DataType.Date)]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DateCreate { set; get; }
        [DataType(DataType.Date)]
        public DateTime DateUpdate { set; get; }
        public int? ParentTicket { set; get; }
        //foreign key and navigation property on Form
        public Category Category { set; get; }
        [DisplayName("От кого")]
        [Required(ErrorMessage = "Поле обязательно")]
        public string PersonId { set; get; }
        [ForeignKey("PersonId")]
        public Person Person { set; get; }
        [DisplayName("Сотрудник")]
        //[Required(ErrorMessage = "Поле обязательно")]
        public string StaffId { set; get; }
        [ForeignKey("StaffId")]
        public Person Stuff { set; get; }
        [DisplayName("Категория заявки")]
        [Required(ErrorMessage = "Поле обязательно")]
        public int CategoryId { set; get; }
        public List<TicketThread> TicketThreads { set; get; }
        public List<FormEntryValue> FormEntryValue { set; get; }
        [DisplayName("Соавторы")]
        public List<CoAuthor> CoAuthors { set; get; }
        public List<Tasks> Tasks { set; get; }
        public List<File> Files { set; get; }
    }
    public class SourceList
    {
        public List<SelectListItem> List { set; get; } = new List<SelectListItem>{
                      new SelectListItem{ Value="1",Text="Телефон" },
                      new SelectListItem{ Value="2",Text="Электронная почта" },
                      new SelectListItem{ Value="3",Text="Письменное обращение" },
                      new SelectListItem{ Value="4",Text="Личный кабинет" },
                //    new SelectListItem{ Value="phone",Text="Номер телефона" },
                //    new SelectListItem{ Value="bool",Text="Флажок" },
                //    new SelectListItem{ Value="choices",Text="Варианты" },
                //    new SelectListItem{ Value="files",Text="Загрузка файла" },
                //    new SelectListItem{ Value="break",Text="Разрыв Секции" },
                //    new SelectListItem{ Value="info",Text="Информация" }
                };
    }
    public class Counters
    {
        public int overlayTicketCount { set; get; }
        public int openTicketCount { set; get; }
        public int myTicketCount { set; get; }
        public int closeTicketCount { set; get; }
        public int noanswerTicketCount { set; get; }
        public int ticketThreadsCount { set; get; }
    }
}
