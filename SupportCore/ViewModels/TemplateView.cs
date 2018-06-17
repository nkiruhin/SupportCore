using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.ViewModels
{
    public class TemplateView
    {
        public string Пользователь { set; get; }
        public int Номер { set; get; }
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime ДатаСоздания { set; get; }
        public string Тема { set; get; }
        //[DataType(DataType.Date)]
        public string ПлановаяДата { set; get; }
        public string Статус { set; get; }
        public string Сотрудник { set; get; }
        [DataType(DataType.Date)]
        public DateTime ДатаЗакрытия { set; get; }
    }
}
