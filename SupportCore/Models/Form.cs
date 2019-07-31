using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class Form
    {
        [ScaffoldColumn(false)]
        public int Id { set; get; }
        [ScaffoldColumn(false)]
        [Display(Name = "Тип")]
        public int Type { set; get; }
        //Type of form
        //0 -System
        //1 -User
        [Required]
        [Display(Name = "Название")]
        public string Name { set; get; }
        [ScaffoldColumn(false)]
        public string Label { set; get; }
        [Display(Name = "Описание")]
        public string Description { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DateCreate { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DateUpdate { set; get; }
        //foreign key and navigation property on Fields
        [ScaffoldColumn(false)]
        public List<Field> Fields { set; get; }
    }
}
