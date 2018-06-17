using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupportCore.Models;
using SupportCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Components
{
    [ViewComponent]
    public class Account : ViewComponent
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public Account(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, Context context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id,string AccountId)
        {
            if (String.IsNullOrEmpty(AccountId)) return View(new AccountView { PersonId = id });
            var UserAccount = await _userManager.FindByIdAsync(AccountId);
            List<SelectListItem> roles = new List<SelectListItem>{
                new SelectListItem { Value="Пользователь", Text= "Пользователь" }
            };
            if (User.IsInRole("Администратор"))
            {
                roles = _roleManager.Roles.Select(n => new SelectListItem { Value = n.Name, Text = n.Name }).ToList();
            }

            AccountView account = new AccountView{
                AccountId = AccountId,
                UserName = UserAccount.UserName,
                Password = UserAccount.PasswordHash,
                RoleName = _userManager.GetRolesAsync(UserAccount).Result.FirstOrDefault(),
                PersonId = id,
                RoleList = roles
            };
            var list = new SelectList(_roleManager.Roles.AsEnumerable(), "Name", "Name");
            return View(account);
        }
    }
}
