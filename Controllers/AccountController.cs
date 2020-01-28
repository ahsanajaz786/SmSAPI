using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiJwt.Configurations;
using WebApiJwt.Entities;
using WebApiJwt.Models;

namespace WebApiJwt.Controllers
{
    [Route("/api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;

        public IHostingEnvironment HostingEnvironment { get; }

        public AccountController(
            UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration =     configuration;
            this._context = context;
            HostingEnvironment = hostingEnvironment;
        }
      
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto model, [FromServices] RedisService redisService,int? school_id=0)
        {
            //Checking if user is activated
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
            {
               

                if (school_id == 0)
                {

                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    if(appUser.role_id==-1)
                    {
                        return Json("admin");
                    }

                    var checkUser = _context.schoo_Users.ToList().Where(o => o.user_Id == appUser.Id).Select(o=>o.school_Id).ToList();
                    if (checkUser.Count > 1)
                    {
                        var list = _context.schools.Where(o => checkUser.Contains(o.id)).ToList();
                        return Json(new
                        {
                            list
                        });

                    }

                    else if (checkUser.Count == 1)
                    {
                       var data = _context.school_Years.FirstOrDefault(o=>checkUser.Contains(o.school_ID));
                        if(data==null)
                        {
                            return Unauthorized();

                        }
                        //    User.AddUpdateClaim("key1", "value1");
                        UserClaimDto claimDto = new UserClaimDto() {
                            Id = appUser.Id,
                            Email = appUser.Email,
                            TimeZone = appUser.Timezone,
                            SchoolId = checkUser.FirstOrDefault(),
                       year_Id = data.id
                        };


                        return new JsonResult(new
                        {
                            
                            token = await GenerateJwtToken(model.Email, claimDto)

                        });
                    }
                    else
                    {
                        return new JsonResult("Unauthorized"
                        );
                    }


                    
                }
                else
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    
                    var checkUser = _context.schoo_Users.Where(o => o.user_Id == appUser.Id && o.school_Id==school_id).Select(o=>o.school_Id).ToList();
                    if (checkUser.Count > 1)
                    {
                        var list = _context.schools.Where(o => checkUser.Contains(o.id)).ToList();
                        return Json(new
                        {
                            list
                        });

                    }

                    else if (checkUser.Count == 1)
                    {
                      var year_Id = _context.school_Years.FirstOrDefault();

                        UserClaimDto claimDto = new UserClaimDto()
                        {
                            Id = appUser.Id,
                            Email = appUser.Email,
                            TimeZone = appUser.Timezone,
                            SchoolId = checkUser.FirstOrDefault(),
                        year_Id=year_Id.id
                        };

                        return new JsonResult(new
                        {
                            token = await GenerateJwtToken(model.Email, claimDto)

                        });
                    }
                    else
                    {
                        return new JsonResult("Unauthorized"
                        );
                    }


                }
            }


            return new JsonResult("Unauthorized"
                        );
        }
       
        [Authorize]
        [HttpGet]
        public IActionResult Me([FromServices] TenantInformation ti)
        {

            var user = _context.Users.Where(e => e.Id == ti.UserId).FirstOrDefault();
            var timeformat = _context.schools.FirstOrDefault();
            return new JsonResult(new
            {
                user = user,
                school=timeformat.name,
                logo=timeformat.imgPath,
                timeformat = timeformat.date_format,
                currency_symbol = timeformat.currency_format,
                currency_name = timeformat.currency_name
            });
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
        
           
        if(model.id==0)
            { 

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
                ,EmailConfirmed=true
                ,role_id=1,
                Timezone = TimeZone.CurrentTimeZone.StandardName.ToString()
                ,profilePath=""

        };
            School_Year school_Year = new School_Year()
            {
               // school_ID = school.id,
                isActive=model.isActive,
                YearID=model.year

            };
            var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    School school = new School();

                    var fileName = "";
                    var uploads = Path.Combine(HostingEnvironment.WebRootPath, "uploads");
                    if (model.schoolLogo.Length > 0)
                    {
                        fileName = Guid.NewGuid().ToString().Replace("-", model.schoolLogo.FileName) + Path.GetExtension(model.schoolLogo.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await model.schoolLogo.CopyToAsync(fileStream);
                            //emp.BookPic = fileName;
                        }

                    }

                    //If the user is a new tenant create tenant 




                    school = new School();
                    school.created_by = user.Id;
                    school.created_date = DateTime.Now;
                    school.name = model.schoolName;
                    school.imgPath = fileName;
                    school.timezone = TimeZone.CurrentTimeZone.StandardName.ToString();
                    _context.Entry(school).State = EntityState.Added;
                    _context.SaveChanges();
                    Schoo_User schoo_User = new Schoo_User()
                    {
                        user_Id = user.Id,
                        school_Id = school.id
                    };


                    _context.Entry(schoo_User).State = EntityState.Added;
                    _context.SaveChanges();


                    school_Year.school_ID = school.id;
                    _context.school_Years.Add(school_Year);
                    _context.SaveChanges();



                    //SendActivationCode(user);

                    return Ok("Done");
                }
                else
                {
                    return new JsonResult(new
                    {
                        Message = "Error",
                        Errors = result.Errors
                    });
                }

                
            }
            else
            {

                 var scholl = _context.schools.FirstOrDefault(o=>o.id==model.id);
                var fileName = "";
                if (model.schoolLogo.Length != null)

                {
                  
                    var deletefile = Path.Combine(HostingEnvironment.WebRootPath+ "/uploads");
                    var uploads = Path.Combine(HostingEnvironment.WebRootPath, "uploads");

                    if (System.IO.File.Exists((Path.Combine(deletefile,scholl.imgPath))))
                    {

                        System.IO.File.Delete((Path.Combine(deletefile, scholl.imgPath)));

                    }


                    fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(model.schoolLogo.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await model.schoolLogo.CopyToAsync(fileStream);
                       
                    }

                }
                scholl.name = model.schoolName;
                scholl.imgPath = fileName;
                _context.Entry(scholl).State = EntityState.Modified;
                _context.SaveChanges();
                var sy = _context.school_Years.FirstOrDefault(o=>o.school_ID==model.id);
                sy.isActive = model.isActive;
                sy.YearID = model.year;
                _context.Entry(sy).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok("Done");


            }


        }
        [HttpGet]
        public void SendActivationCode(User user)
        {
            //Generate activation code and send it to user
            string activationCode = GenerateActivationCode();
            ActivationCode ac = new ActivationCode();
            ac.activationCode = activationCode;
            ac.user_id = user.Id;
            ac.createdDate = DateTime.Now;

            _context.Entry(ac).State = EntityState.Added;
            _context.SaveChanges();

            
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] Dictionary<string, string> model)
        {
            var user = await _userManager.FindByEmailAsync(model["Email"]);

            if (user != null)
            {
                //Generate activation code and send it to user
                string validation_code = GenerateActivationCode();
                Validation_Code vc = new Validation_Code();
                vc.validation_code = validation_code;
                vc.user_id = user.Id;

                _context.Entry(vc).State = EntityState.Added;
                _context.SaveChanges();

                return new JsonResult(new
                {
                    Message = "Success",
                    Success = "A validation code has been sent to your email, use that code to reset you password."
                });
            }
            else
            {
                return new JsonResult( new 
                {
                    Message = "Error",
                    Errors = "Email does not exist."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] Dictionary<string, string> model)
        {
            var user = await _userManager.FindByIdAsync(model["UserID"]);

            if (user != null)
            {
                var newHashedPwd = _userManager.PasswordHasher.HashPassword(user, model["Password"]);
                user.PasswordHash = newHashedPwd;
                await _userManager.UpdateAsync(user);
                return new JsonResult(new
                {
                    Message = "Success",
                    Success = "Password updated successfully."
                });
            }
            else
            {
                return new JsonResult(new
                {
                    Message = "Error",
                    Errors = "Error resetting password."
                });
            }
        }


        [PermissionAuthorize(PermissionNames.AnotherPermission)]
        [HttpGet]
        public IActionResult AnotherRoute()
        {
            return Content("Done");
        }
        
        private async Task<object> GenerateJwtToken(string email, UserClaimDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim("school_id",user.SchoolId.ToString()),
                new Claim("PERMISSION","1"),
                new Claim("UserID",user.Id),
                new Claim("year_id",user.year_Id.ToString()),

                new Claim("Timezone",user.TimeZone)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires, 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        [NonAction]
        private string GenerateActivationCode()
        {
            string activationCode = "";
            Random r = new Random();
            for(int i = 0; i < 15; i++)
            {
                var val = r.Next(10);
                if (val % 2 == 0)
                {
                    //Generate a number and concate it
                    activationCode += r.Next(1, 99).ToString();
                }
                else
                {
                    activationCode += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * r.NextDouble() + 65)));
                }
            }
            return activationCode;
        }

        public class LoginDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

        }
        public class UserClaimDto
        {
            public string Email { get; set; }
            public string Id { get; set; }

            public string TimeZone { get; set; }

            public int SchoolId { get; set; }

            public int year_Id { get; set; }


        }

        public class RegisterDto
        {
            public int id { get; set; }
            public string FullName { get; set; }
            [Required]
            public string Email { get; set; }
            public bool isActive { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }

            public int GroupID { get; set; }
            public int year { get; set; }
            public String schoolName { get; set; }
            public IFormFile schoolLogo { get; set; }
        }






    }
}