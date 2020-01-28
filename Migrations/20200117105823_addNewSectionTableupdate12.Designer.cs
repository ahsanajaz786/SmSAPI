﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiJwt.Entities;

namespace WebApiJwt.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200117105823_addNewSectionTableupdate12")]
    partial class addNewSectionTableupdate12
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WebApiJwt.Entities.Meta_Field", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<int?>("field_id");

                    b.Property<string>("field_type");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("precedence_level");

                    b.Property<int>("school_id");

                    b.Property<int>("sort_id");

                    b.Property<string>("type");

                    b.Property<string>("user_id");

                    b.Property<int>("width");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("Meta_Fields");
                });

            modelBuilder.Entity("WebApiJwt.Entities.Module", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name");

                    b.HasKey("id");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("WebApiJwt.Entities.Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("access_level_view");

                    b.Property<bool>("create_view");

                    b.Property<bool>("delete_view");

                    b.Property<int>("module_id");

                    b.Property<string>("name");

                    b.Property<bool>("read_view");

                    b.Property<bool>("update_view");

                    b.HasKey("id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("WebApiJwt.Entities.Role", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<string>("name");

                    b.Property<int>("school_id");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("WebApiJwt.Entities.Role_Lead_Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("access_level");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<int>("lead_id");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<string>("user_id");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("Role_Lead_Permissions");
                });

            modelBuilder.Entity("WebApiJwt.Entities.Role_Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("access_level");

                    b.Property<bool>("create");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("delete");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("module_id");

                    b.Property<int>("permission_id");

                    b.Property<bool>("read");

                    b.Property<int>("role_id");

                    b.Property<int>("school_id");

                    b.Property<bool>("update");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("Role_Permissions");
                });

            modelBuilder.Entity("WebApiJwt.Entities.Schoo_User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("school_Id");

                    b.Property<string>("user_Id");

                    b.HasKey("id");

                    b.ToTable("schoo_Users");
                });

            modelBuilder.Entity("WebApiJwt.Entities.School", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<string>("currency_format");

                    b.Property<string>("currency_name");

                    b.Property<string>("date_format");

                    b.Property<string>("imgPath");

                    b.Property<string>("name");

                    b.Property<string>("timezone");

                    b.HasKey("id");

                    b.ToTable("schools");
                });

            modelBuilder.Entity("WebApiJwt.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Timezone");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int?>("group_id");

                    b.Property<bool>("is_activated");

                    b.Property<int>("role_id");

                    b.Property<long>("space_left");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WebApiJwt.Entities.User_Lead_Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("access_level");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<int>("lead_id");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<int>("user_id");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("User_Lead_Permissions");
                });

            modelBuilder.Entity("WebApiJwt.Entities.User_Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("access_level");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("module_id");

                    b.Property<int>("school_id");

                    b.Property<string>("user_id");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("User_Permissions");
                });

            modelBuilder.Entity("WebApiJwt.Models.Badge", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<string>("image_Path");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<string>("title");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("badges");
                });

            modelBuilder.Entity("WebApiJwt.Models.Class_Section", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("advisor");

                    b.Property<int>("class_ID");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<string>("section_Name");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("class_Sections");
                });

            modelBuilder.Entity("WebApiJwt.Models.Class_Subject", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("class_section_Id");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<int>("subject_Id");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("class_Subjects");
                });

            modelBuilder.Entity("WebApiJwt.Models.Classes", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("level");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("classes");
                });

            modelBuilder.Entity("WebApiJwt.Models.Global_School_Year", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<DateTime>("end_Date");

                    b.Property<bool>("isActive");

                    b.Property<bool>("is_deleted");

                    b.Property<DateTime>("modified_date");

                    b.Property<DateTime>("start_Date");

                    b.Property<int>("year");

                    b.HasKey("id");

                    b.ToTable("global_School_Years");
                });

            modelBuilder.Entity("WebApiJwt.Models.School_Year", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("YearID");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("isActive");

                    b.Property<bool>("is_deleted");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_ID");

                    b.HasKey("id");

                    b.ToTable("school_Years");
                });

            modelBuilder.Entity("WebApiJwt.Models.Section", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<string>("sectionName");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("section");
                });

            modelBuilder.Entity("WebApiJwt.Models.Student_class", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("class_Section_Id");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<string>("student_Id");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("student_Classes");
                });

            modelBuilder.Entity("WebApiJwt.Models.Subject", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<string>("subject_Name");

                    b.Property<string>("title");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("subjects");
                });

            modelBuilder.Entity("WebApiJwt.Models.SubjectScore", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<double>("score");

                    b.Property<bool>("status");

                    b.Property<string>("student_Id");

                    b.Property<int>("subject_Id");

                    b.Property<string>("teacher_Id");

                    b.Property<int>("todoId");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("subjectScores");
                });

            modelBuilder.Entity("WebApiJwt.Models.Teacher_Subject", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("class_Id");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<int>("section_Id");

                    b.Property<int>("subject_Id");

                    b.Property<string>("teacher_Id");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("teacher_Subjects");
                });

            modelBuilder.Entity("WebApiJwt.Models.Todo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("class_Id");

                    b.Property<string>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<DateTime>("date_Time");

                    b.Property<string>("description");

                    b.Property<string>("fileName");

                    b.Property<string>("fileUploadPath");

                    b.Property<bool>("is_deleted");

                    b.Property<string>("modified_by");

                    b.Property<DateTime>("modified_date");

                    b.Property<int>("school_id");

                    b.Property<double>("score");

                    b.Property<int>("section_Id");

                    b.Property<int>("subject_Id");

                    b.Property<string>("teacher_Id");

                    b.Property<string>("title");

                    b.Property<int>("type");

                    b.Property<int>("year_id");

                    b.HasKey("id");

                    b.ToTable("todos");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WebApiJwt.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WebApiJwt.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApiJwt.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WebApiJwt.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
