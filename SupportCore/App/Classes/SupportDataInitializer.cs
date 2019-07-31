using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.App.Classes
{
    public class SupportDataInitializer
    {
        public static void SeedData(IServiceProvider service)
        {
            var context = service.GetRequiredService<Context>();
            context.Database.Migrate();
            if (!context.Forms.Any())//Add system form in new database
            {
                context.Forms.Add(new Form()
                { Name = "Основная форма ввода",
                  Description = "Системная форма",
                  DateCreate =DateTime.Now,
                  Type=0,
                  Fields=new List<Field>{
                      new Field { Type="html", Name="Описание",Label="description",Private=false,Required=true,DateCreate=DateTime.Now},
                      new Field { Type="select", Name="Приоритет",Configuration="Низкий\r\nСредний\r\nНемедленный",Label="priority",Private=false,Required=true,DateCreate=DateTime.Now}                   
                  }
                }
                );
                context.SaveChanges();
            }           
        }
    }
}
