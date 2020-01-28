using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApiJwt.Configurations;
using WebApiJwt.Configurations.Policies;
using WebApiJwt.Entities;
using WebApiJwt.Models;

namespace WebApiJwt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private string conectionString;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var evn = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //   var en= Environment.UserName;
            if (evn == "Development")
            {
                conectionString = Configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                conectionString = Configuration.GetConnectionString("production");

            }
            // ===== Add our DbContext ========
            services.AddDbContext<ApplicationDbContext>(
                option => option.UseMySql(connectionString: this.conectionString)
                );

            // ===== Add Identity ========
            services.AddIdentity<User, IdentityRole>(
                    config =>
                    {
                        //config.SignIn.RequireConfirmedEmail = true;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //=== Login with Google Option ====


            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            // ===== Add MVC ========
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "EzCRM Api",
                    Version = "1",
                    Description = "EzCRM Beta API Ui Panel"
                });
                c.AddSecurityDefinition("Bearer",
                new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });
            services.AddTransient<ClaimsPrincipal>(
                x =>
                {
                    var currentContext = x.GetService<IHttpContextAccessor>();
                    if (currentContext.HttpContext != null)
                    {
                        return currentContext.HttpContext.User;
                    }
                    else
                    {
                        return null;
                    }
                }
            );
            services.Configure<IdentityOptions>(options =>
            {
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });


            // Policy Configuration
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionPolicy>();
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddTransient<TenantInformation>(
                x =>
                {
                    var currentContext = x.GetService<IHttpContextAccessor>();
                    if (currentContext.HttpContext != null && currentContext.HttpContext.User != null)
                    {
                        var claimsPrincipal = currentContext.HttpContext.User;
                        TenantInformation ti = new TenantInformation();
                        ti.TenantId = claimsPrincipal.Claims.Where(e => e.Type == "school_id").Select(e => e.Value).FirstOrDefault();
                        ti.UserId = claimsPrincipal.Claims.Where(e => e.Type == "UserID").Select(e => e.Value).FirstOrDefault();
                        ti.Timezone = claimsPrincipal.Claims.Where(e => e.Type == "Timezone").Select(e => e.Value).FirstOrDefault();
                        return ti;
                    }
                    else
                    {
                        return null;
                    }
                }
            );
            services.AddTransient<RedisService>();
            services.AddScoped<UserCultureInfo>();
            services.AddMvc(option => option.RegisterDateTimeProvider(services)).AddJsonOptions(jsonOption => jsonOption.RegisterDateTimeConverter(services));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ApplicationDbContext dbContext,
            RedisService redisService
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
            app.UseStaticFiles();
            redisService.Connect();

            // ===== Use Authentication ======
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {

                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "EzCRM_Api");

            });

            // ===== Create tables ======
            dbContext.Database.EnsureCreated();
        }
    }
}
