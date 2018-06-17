using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class Category
    {
        [ScaffoldColumn(false)]
        public int Id { set; get; }
        [Required(ErrorMessage = "Поле обязательно")]
        [DisplayName("Название")]
        public string Name { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DateCreate {set;get;}
        [ScaffoldColumn(false)]
        public DateTime DateUpdate { set; get; }
        [DisplayName("Описание")]
        public string Discription { set; get; }
        [DisplayName("Форма")]
        public int? FormId { set; get; }
        [ScaffoldColumn(false)]
        [DisplayName("Форма")]
        public Form Form { set; get; }
        [ScaffoldColumn(false)]
        public bool IsActive { set; get; }
        [ScaffoldColumn(false)]
        public bool IsDeleted { set; get; }
    }
}
