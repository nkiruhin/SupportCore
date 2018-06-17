using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class Field
    {
        public int Id { set; get; }
        [Display(Name = "Тип")]
        [Required(ErrorMessage = "Поле обязательно")]
        public string Type { set; get; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле обязательно")]
        public string Name { set; get; }
        [Display(Name = "Обязательность")]
        public bool Required { set; get; }
        public bool Private { set; get; }
        public string Mask { set; get; }
        public string Label { set; get; }
        [DataType(DataType.MultilineText)]
        public string Configuration { set; get; }
        public DateTime DateCreate { set; get; }
        public DateTime DateUpdate { set; get; }
        //foreign key and navigation property on Form
        [HiddenInput(DisplayValue = false)]
        public int FormId { set; get; }
        public Form Form { set; get; }
        public List<FormEntryValue> FormEntryValue { set;get;}
    }
}
