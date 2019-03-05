using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SupportCore.ViewModels;
using SupportCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SupportCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Context _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        //public async Task<IActionResult> CreateRole(string Name)
        //{
        //    var role = new IdentityRole
        //    {
        //        Name = Name
        //    };
        //    var result = await _roleManager.CreateAsync(role);
        //    return Content("Роль " + Name + " успешно создана");
        //}
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            return View(new AccountView { ReturnUrl = ReturnUrl });
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // GET: /Account/Index
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountView model)
        {
            //var user = new User { UserName = model.UserName, };
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction(nameof(HomeController.Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин и (или) пароль");
                }
            }
            return View(model);
        }
        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register(string id)
        {
            List<SelectListItem> roles = new List<SelectListItem>{
                new SelectListItem { Value="Пользователь", Text= "Пользователь" }
            };
            if (User.IsInRole("Администратор"))
            {
                 roles = _roleManager.Roles.Select(n => new SelectListItem { Value = n.Name, Text = n.Name }).ToList();
            }
            var account = new AccountView
            {
                RoleList = roles,
                PersonId = id
            };
            return PartialView(account);
        }
        // POST: /Account/Register
        [HttpPost]
       // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountView model)
        {
            model.RoleName = HttpContext.Request.Form["RoleList"];
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user,model.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Ошибка при создании учетной записи:");
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    List<SelectListItem> roles = new List<SelectListItem>{
                            new SelectListItem { Value="Пользователь", Text= "Пользователь" }
                    };
                    if (User.IsInRole("Администратор"))
                    {
                        roles = _roleManager.Roles.Select(n => new SelectListItem { Value = n.Name, Text = n.Name }).ToList();
                    }
                    var account = new AccountView
                    {
                        RoleList = roles,
                        PersonId = model.PersonId
                    };
                    return PartialView(account);
                }
                await _userManager.AddToRoleAsync(user, model.RoleName);
                var person = await _context.Person.SingleOrDefaultAsync(p => p.Id == model.PersonId);
                person.AccountID = await _userManager.GetUserIdAsync(user);
                await _context.SaveChangesAsync();
            }          
            return RedirectToAction("Register",new { id = model.PersonId });
        }
        // POST: /Account/Delete
        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string PersonId,string AccountId)
        {
            var User = await _userManager.FindByIdAsync(AccountId);
            await _userManager.DeleteAsync(User);
            var person = await _context.Person.SingleOrDefaultAsync(p => p.Id == PersonId);
            person.AccountID = null;
            await _context.SaveChangesAsync();
            return ViewComponent("Account", new { id = PersonId, AccountId = String.Empty });
        }
        // POST: /Account/EditAccount
        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountView NewAccount)
        {
            var User = await _userManager.FindByIdAsync(NewAccount.AccountId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(User);
            var result = await _userManager.ResetPasswordAsync(User,token,NewAccount.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при создании учетной записи:");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return ViewComponent("Account", new { id = NewAccount.PersonId, AccountId = NewAccount.AccountId });
                }
            }
            var Roles = await _userManager.GetRolesAsync(User);
            if (Roles.FirstOrDefault() != NewAccount.RoleName)
            {
                await _userManager.RemoveFromRolesAsync(User, Roles);
                await _userManager.AddToRoleAsync(User, NewAccount.RoleName);
            }
            return ViewComponent("Account", new { id = NewAccount.PersonId, AccountId = NewAccount.AccountId });
        }
    }
}