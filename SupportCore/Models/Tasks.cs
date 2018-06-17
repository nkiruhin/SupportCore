using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class Tasks
    {
        [ScaffoldColumn(false)]
        public int Id { set; get; }
        [DisplayName("Дата")]
        public DateTime DateCreate { set; get; }
        [DisplayName("Дата закрытия")]
        [DataType(DataType.Date)]
        public DateTime DateClose { set; get; }
        [DisplayName("ссылка/№ у исполнителя")]
        [DataType(DataType.Url)]
        public string Number { set; get; }
        [DisplayName("Организация")]
        [ScaffoldColumn(false)]
        public int? OrganizationId { set; get; }
        [ScaffoldColumn(false)]
        [DisplayName("Организация")]
        public Organization Organization { set; get; }
        [ForeignKey("StaffId")]
        [DisplayName("Сотрудник")]
        public Person Staff { set; get; }
        [DisplayName("Сотрудник")]
        public string StaffId { set; get; }
        [DisplayName("Статус")]
        public int Status { set; get; }
        [HiddenInput(DisplayValue = false)]
        public int TicketId { set; get; }
        [DisplayName("Тема")]
        [Required(ErrorMessage ="Поле обязательно")]
        public string Title { set; get; }
        [ScaffoldColumn(false)]
        public int Type { set; get; }
        [Required(ErrorMessage = "Поле обязательно")]
        [DisplayName("Описание")]
        [DataType(DataType.Html)]
        public string Body { set; get; }
        [DisplayName("Результат выполнения")]
        public string Result { set; get; }
    }
}
