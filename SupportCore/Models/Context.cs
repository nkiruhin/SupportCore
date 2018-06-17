using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SupportCore.Models
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        { }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketThread> TicketThreads { get; set; }
        public DbSet<FormEntryValue> FormEntryValues { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<CoAuthor> CoAuthors { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<File> Files { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuration>().HasData(
             new { Name = "Signature", Value = "С Уважением служба технической поддержки", Section = "email", Title = "Подпись", Type = "html" },
             new { Name = "FromAddress",Value="",Section="email",Title= "Адресс отправителя",Type="text" },
             new { Name = "FromName", Value = "", Section = "email", Title = "От кого", Type = "text" },
             new { Name = "UserId", Value = "", Section = "email", Title = "Имя пользователя", Type = "text" },
             new { Name = "UserPassword", Value = "", Section = "email", Title = "Пароль", Type = "password" }
             );
            modelBuilder.Entity<Form>()
                .HasIndex(b => b.Type);
            modelBuilder.Entity<Form>()
                .Property(b => b.Type)
                .HasDefaultValue(1);
            modelBuilder.Entity<Field>()
                .Property(b => b.Type)
                .HasDefaultValue(1);
            modelBuilder.Entity<Ticket>()
                .HasIndex(b => b.Name);
            modelBuilder.Entity<Filter>()
               .HasIndex(b => b.UserId);
        }
    }
}
