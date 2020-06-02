﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SupportCore.Models;

namespace SupportCore.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SupportCore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<string>("Description");

                    b.Property<int?>("FormId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FormId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("SupportCore.Models.CoAuthor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("PersonId");

                    b.Property<int>("TicketId");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("TicketId");

                    b.ToTable("CoAuthors");
                });

            modelBuilder.Entity("SupportCore.Models.Configuration", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Section");

                    b.Property<string>("Title");

                    b.Property<string>("Type");

                    b.Property<string>("Value");

                    b.HasKey("Name");

                    b.ToTable("Configuration");

                    b.HasData(
                        new
                        {
                            Name = "Signature",
                            Section = "email",
                            Title = "Подпись",
                            Type = "html",
                            Value = "С Уважением служба технической поддержки"
                        },
                        new
                        {
                            Name = "FromAddress",
                            Section = "email",
                            Title = "Адресс отправителя",
                            Type = "text",
                            Value = ""
                        },
                        new
                        {
                            Name = "FromName",
                            Section = "email",
                            Title = "От кого",
                            Type = "text",
                            Value = ""
                        },
                        new
                        {
                            Name = "UserId",
                            Section = "email",
                            Title = "Имя пользователя",
                            Type = "text",
                            Value = ""
                        },
                        new
                        {
                            Name = "UserPassword",
                            Section = "email",
                            Title = "Пароль",
                            Type = "password",
                            Value = ""
                        });
                });

            modelBuilder.Entity("SupportCore.Models.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Configuration");

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<int>("FormId");

                    b.Property<string>("Label");

                    b.Property<string>("Mask");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("Private");

                    b.Property<bool>("Required");

                    b.Property<string>("Type")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("1");

                    b.HasKey("Id");

                    b.HasIndex("FormId");

                    b.HasIndex("Label");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("SupportCore.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentType");

                    b.Property<DateTime>("CreatedTimestamp");

                    b.Property<string>("Description");

                    b.Property<string>("FileName");

                    b.Property<int>("TicketId");

                    b.Property<string>("TicketThreadId");

                    b.Property<DateTime>("UpdatedTimestamp");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.HasIndex("TicketThreadId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("SupportCore.Models.Filter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<DateTime?>("Closed1");

                    b.Property<DateTime?>("Closed2");

                    b.Property<DateTime?>("DateCreate1");

                    b.Property<DateTime?>("DateCreate2");

                    b.Property<DateTime?>("DueDate1");

                    b.Property<DateTime?>("DueDate2");

                    b.Property<string>("Name");

                    b.Property<string>("PersonId");

                    b.Property<string>("Priority");

                    b.Property<int>("SourceId");

                    b.Property<string>("StaffId");

                    b.Property<int>("StatusId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Filters");
                });

            modelBuilder.Entity("SupportCore.Models.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<string>("Description");

                    b.Property<string>("Label");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("Type");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("SupportCore.Models.FormEntryValue", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FieldId");

                    b.Property<int>("TicketId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("TicketId");

                    b.ToTable("FormEntryValues");
                });

            modelBuilder.Entity("SupportCore.Models.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(256);

                    b.Property<string>("Contract")
                        .HasMaxLength(20);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("CuratorId");

                    b.Property<DateTime>("DeleteTime");

                    b.Property<DateTime>("EditTime");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int?>("SLAId");

                    b.Property<string>("Telephone")
                        .IsRequired();

                    b.Property<bool>("isDeleted");

                    b.Property<bool>("isProvider");

                    b.HasKey("Id");

                    b.HasIndex("CuratorId")
                        .IsUnique()
                        .HasFilter("[CuratorId] IS NOT NULL");

                    b.HasIndex("SLAId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("SupportCore.Models.Person", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountID");

                    b.Property<string>("ApiKey");

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsStaff");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("OrganizationId");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("SupportCore.Models.Phone", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PersonId");

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Phone");
                });

            modelBuilder.Entity("SupportCore.Models.Requests", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<string>("Email");

                    b.Property<int>("From");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("PersonId");

                    b.Property<string>("Phone");

                    b.Property<string>("Subject");

                    b.Property<string>("Text");

                    b.Property<int>("TicketId");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("SupportCore.Models.SLA", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryId");

                    b.Property<int?>("DeadTime");

                    b.Property<int?>("FieldId");

                    b.Property<string>("FieldValue");

                    b.Property<int>("IsDefault")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.Property<int>("ResponseTime");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("FieldId");

                    b.ToTable("SLAs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDefault = 1,
                            Name = "Базовый SLA",
                            ResponseTime = 48,
                            Type = 0
                        });
                });

            modelBuilder.Entity("SupportCore.Models.Tasks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<DateTime>("DateClose");

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("Number");

                    b.Property<int?>("OrganizationId");

                    b.Property<string>("Result");

                    b.Property<string>("StaffId");

                    b.Property<int>("Status");

                    b.Property<int>("TicketId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("StaffId");

                    b.HasIndex("TicketId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("SupportCore.Models.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<int>("EventType");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("SupportCore.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("Closed");

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<DateTime>("DueDate");

                    b.Property<int>("Flags");

                    b.Property<bool>("IsAnswered");

                    b.Property<bool>("IsInform");

                    b.Property<bool>("IsOverdue");

                    b.Property<DateTime>("LastMessage");

                    b.Property<DateTime>("LastResponse");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentTicket");

                    b.Property<string>("PersonId")
                        .IsRequired();

                    b.Property<DateTime>("Reopened");

                    b.Property<int>("SourceId");

                    b.Property<string>("StaffId");

                    b.Property<int>("StatusId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DateCreate");

                    b.HasIndex("Name");

                    b.HasIndex("PersonId");

                    b.HasIndex("StaffId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("SupportCore.Models.TicketThread", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<DateTime>("DateCreate");

                    b.Property<DateTime>("DateUpdate");

                    b.Property<bool>("IsInform");

                    b.Property<string>("PersonId");

                    b.Property<string>("Poster");

                    b.Property<int>("SourceId");

                    b.Property<int>("TicketId");

                    b.Property<string>("Title");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketThreads");
                });

            modelBuilder.Entity("SupportCore.Models.Category", b =>
                {
                    b.HasOne("SupportCore.Models.Form", "Form")
                        .WithMany()
                        .HasForeignKey("FormId");
                });

            modelBuilder.Entity("SupportCore.Models.CoAuthor", b =>
                {
                    b.HasOne("SupportCore.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.HasOne("SupportCore.Models.Ticket")
                        .WithMany("CoAuthors")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SupportCore.Models.Field", b =>
                {
                    b.HasOne("SupportCore.Models.Form", "Form")
                        .WithMany("Fields")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SupportCore.Models.File", b =>
                {
                    b.HasOne("SupportCore.Models.Ticket", "Ticket")
                        .WithMany("Files")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SupportCore.Models.TicketThread", "TicketThread")
                        .WithMany("Files")
                        .HasForeignKey("TicketThreadId");
                });

            modelBuilder.Entity("SupportCore.Models.FormEntryValue", b =>
                {
                    b.HasOne("SupportCore.Models.Field", "Field")
                        .WithMany("FormEntryValue")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SupportCore.Models.Ticket", "Ticket")
                        .WithMany("FormEntryValue")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SupportCore.Models.Organization", b =>
                {
                    b.HasOne("SupportCore.Models.Person", "Curator")
                        .WithOne("Organization")
                        .HasForeignKey("SupportCore.Models.Organization", "CuratorId");

                    b.HasOne("SupportCore.Models.SLA", "SLA")
                        .WithMany()
                        .HasForeignKey("SLAId");
                });

            modelBuilder.Entity("SupportCore.Models.Phone", b =>
                {
                    b.HasOne("SupportCore.Models.Person", "Person")
                        .WithMany("Phones")
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("SupportCore.Models.Requests", b =>
                {
                    b.HasOne("SupportCore.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("SupportCore.Models.SLA", b =>
                {
                    b.HasOne("SupportCore.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("SupportCore.Models.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId");
                });

            modelBuilder.Entity("SupportCore.Models.Tasks", b =>
                {
                    b.HasOne("SupportCore.Models.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.HasOne("SupportCore.Models.Person", "Staff")
                        .WithMany()
                        .HasForeignKey("StaffId");

                    b.HasOne("SupportCore.Models.Ticket")
                        .WithMany("Tasks")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SupportCore.Models.Ticket", b =>
                {
                    b.HasOne("SupportCore.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SupportCore.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SupportCore.Models.Person", "Staff")
                        .WithMany()
                        .HasForeignKey("StaffId");
                });

            modelBuilder.Entity("SupportCore.Models.TicketThread", b =>
                {
                    b.HasOne("SupportCore.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.HasOne("SupportCore.Models.Ticket", "Ticket")
                        .WithMany("TicketThreads")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
