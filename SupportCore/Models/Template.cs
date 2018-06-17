using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class Template
    {
        public int Id { set; get; }
        [Display(Name="Имя")]
        public string Name { set; get; }
        [Display(Name = "Действие")]
        public int EventType { set; get; }
        [Display(Name = "Текст")]
        public string Body { set; get; }
        [Display(Name = "По умолчанию для действия")]
        public bool IsDefault { set; get; }
    }
}
