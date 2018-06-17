using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SupportCore.Models
{
    public class Person
    {
        [ScaffoldColumn(false)]
        public string Id { set; get; }
        [Required(ErrorMessage = "Поле обязательно")]
        [Display(Name = "ФИО")]
        public string Name { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DateCreate { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DateUpdate { set; get; }
        [ScaffoldColumn(false)]
        public bool IsDeleted { set; get; }
        [HiddenInput(DisplayValue = false)]
        public string AccountID { set; get; }
        [Required(ErrorMessage = "Поле обязательно")]
        public string Email { set; get; }
        [UIHint("AdminField")]
        //[IsAdmin]
        [Display(Name = "Сотрудник")]
        public bool IsStaff { set; get; }
        [Display(Name = "Ключ api redmine")]
        [ScaffoldColumn(false)]
        public string ApiKey { set; get; }
        [ScaffoldColumn(false)]
        [Display(Name = "Телефоны")]
        public List<Phone> Phones {set;get;}
        //foreign key and navigation property on Organization
        [ScaffoldColumn(false)]
        [Display(Name = "Организация")]
        public int? OrganizationId { set; get; }
        [ScaffoldColumn(false)]
        public Organization Organization { set; get; }
    }
    public class Phone
    {
        [ScaffoldColumn(false)]
        public string Id { set; get; }
        [Required]
        public string PhoneNumber { set; get; }
        //foreign key and navigation property on Person
        [ScaffoldColumn(false)]
        public string PersonId { set; get; }
        [ScaffoldColumn(false)]
        public Person Person { set; get; }
    }
}
