using System;
using System.ComponentModel.DataAnnotations;

namespace SupportCore.Models
{
    public class Requests
    {
        [ScaffoldColumn(false)]
        public string Id { set; get; }
        [Display(Name = "Дата")]
        public DateTime DateCreate { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DateUpdate { set; get; }
        [DataType(DataType.EmailAddress)]
        [Display(Name ="От кого")]
        public string Email { set; get; }
        [ScaffoldColumn(false)]
        public int From { set; get; }
        [ScaffoldColumn(false)]
        public bool IsDeleted { set; get; }
        [ScaffoldColumn(false)]
        public string PersonId { set; get; }
        [ScaffoldColumn(false)]
        public Person Person { set; get; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { set; get; }
        [Display(Name = "Тема")]
        public string Subject { set; get; }
        [DataType(DataType.Html)]
        [Display(Name = "Текст сообщения")]
        public string Text { set; get; }
        [ScaffoldColumn(false)]
        public int TicketId { set; get; }
    }
}