using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiJwt.Entities
{
    public partial class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Meta_Field> Meta_Fields { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role_Permission> Role_Permissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<User_Permission> User_Permissions { get; set; }
        public DbSet<Role_Lead_Permission> Role_Lead_Permissions { get; set; }
        public DbSet<User_Lead_Permission> User_Lead_Permissions { get; set; }

    }

    public class Permission
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int module_id { get; set; }
        public bool create_view { get; set; }
        public bool update_view { get; set; }
        public bool read_view { get; set; }
        public bool delete_view { get; set; }
        public bool access_level_view { get; set; }
    }

    public class School
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string imgPath { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string date_format { get; set; }
        public string currency_name { get; set; }
        public string currency_format { get; set; }
        public string timezone { get; set; }

    }

    public class Schoo_User
    {
        [Key]
        public int id { get; set; }
        public string user_Id { get; set; }
        public int school_Id { get; set; }

    }

    /*
     * -1 -> FullName
     * -2 => Email
     * -3 => PhoneFsc
     * These keys will be used to store fixed fields in this table and later can be retrieved using left outer join
     */

    public class Meta_Field : BaseEntity
    {
        public string type { get; set; }

        public int? field_id { get; set; }
        public int width { get; set; }
        public string user_id { get; set; }
        public int precedence_level { get; set; }
        public int sort_id { get; set; }
        public string field_type { get; set; }
    }

    public class Role : BaseEntity
    {
        public string name { get; set; }
    }

    public class Module
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Role_Permission : BaseEntity
    {
        public int role_id { get; set; }
        public int module_id { get; set; }
        public int permission_id { get; set; }
        public bool create { get; set; }
        public bool update { get; set; }
        public bool read { get; set; }
        public bool delete { get; set; }
        public string access_level { get; set; }
    }

    public class User_Permission : BaseEntity
    {
        public string user_id { get; set; }
        public int module_id { get; set; }
        public string access_level { get; set; }
    }
    
    public class User_Lead_Permission : BaseEntity
    {
        public int lead_id { get; set; }
        public int user_id { get; set; }
        public string access_level { get; set; }
    }

    public class Role_Lead_Permission : BaseEntity
    {
        public int lead_id { get; set; }
        public string user_id { get; set; }
        public string access_level { get; set; }
    }
}
