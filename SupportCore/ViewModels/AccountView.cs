using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.ViewModels
{
    public class AccountView
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        //[Required]
        //[Compare("Password", ErrorMessage = "Пароли не совпадают")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Подтвердить пароль")]
        //public string PasswordConfirm { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string PersonId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string AccountId { get; set; }
        [ScaffoldColumn(false)]
        public string RoleName { get; set; }
        [Display(Name = "Права доступа")]
        public List<SelectListItem> RoleList { get; set; }
        [ScaffoldColumn(false)]
        public string ReturnUrl { get; set; }
    }
}
