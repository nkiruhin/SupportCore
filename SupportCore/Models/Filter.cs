using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class Filter
    {
        public int Id { set; get; }
        public string UserId { set; get; }
        [DisplayName("Тема")]
        public string Name { set; get; }
        [DisplayName("Статус")]
        public int StatusId { set; get; }
        [DisplayName("Источник заявки")]
        public int SourceId { set; get; }
        [DisplayName("Плановая дата от:")]
        [DataType(DataType.Date)]
        public DateTime? DueDate1 { set; get; }
        [DisplayName("Плановая дата до:")]
        [DataType(DataType.Date)]
        public DateTime? DueDate2 { set; get; }
        [DisplayName("Дата закрытия от:")]
        [DataType(DataType.Date)]
        public DateTime? Closed1 { set; get; }
        [DisplayName("Дата закрытия до:")]
        [DataType(DataType.Date)]
        public DateTime? Closed2 { set; get; }
        [DisplayName("Дата открытия от:")]
        [DataType(DataType.Date)]
        public DateTime? DateCreate1 { set; get; }
        [DisplayName("Дата открытия до:")]
        [DataType(DataType.Date)]
        public DateTime? DateCreate2 { set; get; }
        [DisplayName("От кого")]
        public string PersonId { set; get; }
        [DisplayName("Сотрудник")]
        public string StaffId { set; get; }
        [DisplayName("Категория заявки")]
        public int CategoryId { set; get; }
        [DisplayName("Приоритет")]
        public string Priority { set; get; }
        [ScaffoldColumn(false)]//  hidden ticket for Users 
        [NotMapped]
        [DisplayName("Информирование")]
        public bool isInform { set; get; }
        [NotMapped]
        [DisplayName("С ответом")]
        public bool IsAnswered { set; get; }
    }
}
