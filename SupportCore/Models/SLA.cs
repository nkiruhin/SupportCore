using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SupportCore.Models
{
    public class SLA
    {
        public int Id { set; get; }
        [DisplayName("Имя")]
        public string Name { set; get; }
        public int IsDefault { set; get; }
        public int Type { set; get; }
        // Тип SLA 
        // 0 - основной тип
        // 1 - Правило
        [DisplayName("Время реализации")]
        public int? DeadTime { set; get; }
        [DisplayName("Время ответа")]
        public int ResponseTime { set; get; }
        [HiddenInput(DisplayValue = false)]
        public int? ParentId { set; get; }
        [DisplayName("Поле формы")]
        public int? FieldId { set; get; }
        [DisplayName("Значение поля")]
        public string FieldValue { set; get; }
        List<Organization> Organizations { set; get; }
        public Category Category { set; get; }
        [DisplayName("Категория")]
        public int? CategoryId { set; get; }
        public Field Field { set; get; }
    }
}
