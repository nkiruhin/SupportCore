using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace SupportCore.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly Context _context;
        public HomeController(Context context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var person = _context.Person.AsNoTracking().SingleOrDefault(p => p.AccountID == _userManager.GetUserId(HttpContext.User));
                ViewData["UserName"] = person?.Name;
                ViewData["UserId"] = person?.Id;
            }
            ////Administrator menu Item
            var Configuration = new Configuration();
            //Configuration.Menu = new List<AdminMenuItem>
            //{
            //   //new AdminMenuItem{Name = "Users",ControllerName = "User"},
            //   new AdminMenuItem{Name = "Система",ControllerName = "System"}
            //};

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
