using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebApiJwt.Models;

namespace WebApiJwt.Entities
{
    public partial class ApplicationDbContext : IdentityDbContext<User>
    {
        #region Multi_Tenancy_Logic
        private readonly ClaimsPrincipal _loggedInUser = null;

        private static IList<Type> _entityTypeCache;
        private int _tenant_id { get; set; }
        private int _year_id { get; set; }

        private string _loggedIn { get; set; }
        private static IList<Type> GetEntityTypes()
        {
            if (_entityTypeCache != null)
            {
                return _entityTypeCache.ToList();
            }

            _entityTypeCache = (from a in GetReferencingAssemblies()
                                from t in a.DefinedTypes
                                where t.BaseType == typeof(BaseEntity)
                                select t.AsType()).ToList();

            return _entityTypeCache;
        }

        private static IEnumerable<Assembly> GetReferencingAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                { }
            }
            return assemblies;
        }

        public ApplicationDbContext(ClaimsPrincipal c, DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this._loggedInUser = c;
            if (c != null)
            {
                var tenantClaim = c.Claims.Where(e => e.Type == "school_id").FirstOrDefault();
                if (tenantClaim != null)
                {
                    this._tenant_id = int.Parse(tenantClaim.Value);
                }
                var yearClaim = c.Claims.Where(e => e.Type == "year_id").FirstOrDefault();
                if (yearClaim != null)
                {
                    this._year_id = int.Parse(yearClaim.Value);
                }
                tenantClaim = c.Claims.Where(e => e.Type == "UserID").FirstOrDefault();
                if (tenantClaim != null)
                {
                    this._loggedIn = tenantClaim.Value;
                }

            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseMySql(GetConnectionString());
        }

        partial void RegisterSP(ModelBuilder modelBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in GetEntityTypes())
            {

                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }

            this.RegisterSP(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }


        static readonly MethodInfo SetGlobalQueryMethod = typeof(ApplicationDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                                .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseEntity
        {
            if (this._loggedInUser != null)
            {
                builder.Entity<T>().HasKey(e => e.id);
                //Debug.WriteLine("Adding global query for: " + typeof(T));
                builder.Entity<T>().HasQueryFilter(e => e.school_id == _tenant_id && !e.is_deleted);
            }
        }

        private static string GetConnectionString()
        {
            const string databaseName = "schoolmgt";
            const string databaseUser = "root";
            const string databasePass = "";

            return $"Server=localhost; Port=3306;" +
                   $"database={databaseName};" +
                   $"uid={databaseUser};" +
                   $"pwd={databasePass};" +
                   $"pooling=true;";
        }
        #endregion

        #region DB_Specific_Configuration
        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override void AddRange(IEnumerable<object> entities)
        {
            UpdateSoftDeleteStatuses();
            base.AddRange(entities);
        }
        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateSoftDeleteStatuses();
            return base.AddRangeAsync(entities, cancellationToken);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            if (_tenant_id == 0)
                return;
            foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity.GetType().BaseType == typeof(BaseEntity)))
            {
                try
                {
                    if (entry.State == EntityState.Added)
                    {
                        var createdDate = entry.CurrentValues["created_date"] as DateTime?;
                        {
                            entry.CurrentValues["created_date"] = DateTime.Now;
                            entry.CurrentValues["created_by"] = this._loggedIn;
                            entry.CurrentValues["school_id"] = this._tenant_id;
                            entry.CurrentValues["year_id"] = this._year_id;

                        }
                    }

                    entry.CurrentValues["modified_date"] = DateTime.Now;
                    entry.CurrentValues["modified_by"] = this._loggedIn;
                }
                catch (Exception ex)
                {

                }

                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["is_deleted"] = true;
                        break;
                }
            }
        }
        #endregion
        public DbSet<WebApiJwt.Models.SubjectScore> subjectScores { get; set; }
        public DbSet<School> schools { get; set; }
        public DbSet<Schoo_User> schoo_Users { get; set; }
        public DbSet<Todo> todos { get; set; }
        public DbSet<Classes> classes { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<Class_Section> class_Sections { get; set; }
        public DbSet<Class_Subject> class_Subjects { get; set; }
        public DbSet<School_Year> school_Years { get; set; }
        public DbSet<Badge> badges { get; set; }
        public DbSet<Teacher_Subject> teacher_Subjects { get; set; }
        public DbSet<Global_School_Year> global_School_Years { get; set; }
        public DbSet<Student_class> student_Classes { get; set; }


        //  updated
        public DbSet<Student> students { get; set; }
        public DbSet<Section> section { get; set; }
        public DbSet<Teacher> teachers { get; set; }
        public DbSet<Event> events { get; set; }
        public DbSet<ClassEvent> classEvents { get; set; }
        public DbSet<EventAttachment> eventAttachments { get; set; }


    }

    public abstract class BaseEntity
    {
        public int id { get; set; }
        public int school_id { get; set; }
        public bool is_deleted { get; set; }
        public string created_by { get; set; }
        public int year_id { get; set; }
        public DateTime created_date { get; set; }
        [JsonIgnore]
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; } = DateTime.Now;
    }
}