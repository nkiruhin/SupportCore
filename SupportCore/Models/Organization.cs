using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportCore.Models
{
    public class Organization
    {
        [ScaffoldColumn(false)]
        public int Id { set; get; }
        [Required(ErrorMessage = "Поле обязательно")]
        [Display(Name = "Наименование")]
        [MaxLength(128)]
        public string Name { set; get; }
        [Display(Name = "Контракт")]
        [MaxLength(20)]
        public string Contract { set; get; }
        [DataType(DataType.MultilineText)]
        [MaxLength(256)]
        [Display(Name = "Адрес")]
        public string Address { set; get; }
        [Required(ErrorMessage = "Поле обязательно")]
        [EmailAddress]
        public string Email { set; get; }   
        [Required(ErrorMessage = "Поле обязательно")]
        [Phone(ErrorMessage ="Неверный формат")]
        [Display(Name = "Телефон")]
        public string Telephone { set; get; }
        [Display(Name = "Ответственный сотрудник")]
        public string CuratorId { set; get; }
        [ScaffoldColumn(false)]
        [ForeignKey("CuratorId")]
        [Display(Name = "Куратор")]
        public Person Curator { set; get; }     
        [ScaffoldColumn(false)]
        public SLA SLA { set; get; }
        [Display(Name = "SLA")]
        public int? SLAId { set; get; }             
        [Display(Name = "Подрядчик")]
        public bool isProvider { set; get; }
        [ScaffoldColumn(false)]
        public DateTime CreateTime { set; get; }
        [ScaffoldColumn(false)]
        public DateTime EditTime { set; get; }
        [ScaffoldColumn(false)]
        public Boolean isDeleted { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DeleteTime { set; get; }
        //principal entity 
        [ScaffoldColumn(false)]
        [NotMapped]
        public List<Person> Persons { set; get; }
    }
}
