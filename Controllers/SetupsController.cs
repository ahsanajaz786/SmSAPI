using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiJwt.Entities;
using WebApiJwt;
using System.ComponentModel.DataAnnotations;
using WebApiJwt.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using WebApiJwt.Models;
using Microsoft.AspNetCore.Identity;

namespace WebApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupsController : ControllerBase
    {

        ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public UserManager<User> UserManager { get; }

        public SetupsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, UserManager<User> userManager)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
            UserManager = userManager;
        }
        
        
        
        
        
        
        

        [HttpPost]
        [Route("GatSchoolAdmin")]
        public JsonResult GatSchoolAdmin(int? id = null)
        {
            if (id == null)
            {
                var dat = (from k in _context.schoo_Users
                           
                           join l in _context.Users on k.user_Id equals l.Id
                           where l.role_id==1
                           join o in _context.schools on k.school_Id equals o.id
                           select new
                           {

                               k.id,
                               l.FullName,
                               o.imgPath,

                               l.UserName,
                               l.Email,
                               o.name
                           }
                         ).ToList();
                return new JsonResult(dat);
            }
            else
            {
                var dat = (from k in _context.schoo_Users
                           join l in _context.Users on k.user_Id equals l.Id
                           where l.role_id == 1
                           join o in _context.schools on k.school_Id equals o.id
                           join u in _context.school_Years on o.id equals u.school_ID
                           join y in _context.global_School_Years on u.YearID equals y.id 
                           select new
                           {
                               k.id,

                               schooid=o.id,
                               fn = l.FullName,
                               o.imgPath,
                              year= y.id,
                             active=u.isActive
                              ,
                               l.UserName,
                               em = l.Email,
                               o.name
                           }
                        ).FirstOrDefault(o => o.id == id);
                return new JsonResult(dat);
            }
        }
      
        [HttpGet("GetSchoolYear")]
        public JsonResult GetSchoolYear(int? id = 0)
        {
            if (id == 0)
            {
                var data = (from k in _context.global_School_Years

                            //join l in _context.schools on k.school_ID equals l.id
                            select new
                            {
                                k.id,
                                k.year,
                                startDate=k.start_Date.ToShortDateString(),
                                endDate=k.end_Date.ToShortDateString(),
                               a= k.isActive
                            }
                              ).ToList();
                return new JsonResult(data);

            }
            else if (id == -1)
            {
                var data = (from k in _context.global_School_Years
                                //join l in _context.schools on k.school_ID equals l.id
                            select new
                            {
                                k.id,
                                k.year,
                                startDate = k.start_Date.ToShortDateString(),
                                endDate = k.end_Date.ToShortDateString(),
                                a = k.isActive
                            }
                              ).Where(o=>o.a==true).ToList();
                return new JsonResult(data);

            }
            else
            {
             var data=   (from k in _context.global_School_Years
                     //join l in _context.schools on k.school_ID equals l.id
                 select new
                 {
                     k.id,
                     k.year,
                     startDate = k.start_Date.ToShortDateString(),
                     endDate = k.end_Date.ToShortDateString(),
                     a = k.isActive
                 }
                               ).FirstOrDefault(o=>o.id==id);

                return new JsonResult(data);
            }
        }
        [HttpPost("AddSchoolYear")]
        public JsonResult AddSchoolYear([FromBody]Global_School_YearDTO global_School_Year)

        {
            if (ModelState.IsValid)
            {
                if (global_School_Year.id == 0)
                {
                    DateTime startDate =DateTime.Parse(global_School_Year.start_Date);
                    DateTime endDate = DateTime.Parse(global_School_Year.end_Date);
                    Global_School_Year g = new Global_School_Year()
                    {
                        year=global_School_Year.year,
                        start_Date=startDate,
                        end_Date=endDate,
                        isActive =global_School_Year.isActive
                    };

                    _context.global_School_Years.Add(g);
                    _context.SaveChanges();
                    return new JsonResult(true);
                }
                else
                {
                  //  var global_School_ = _context.global_School_Years.FirstOrDefault(o=>o.id==global_School_Year.id);

                  //  global_School_.isActive = global_School_Year.isActive;

                    _context.Entry(global_School_Year).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    return new JsonResult(true);
                }
              //  return new JsonResult("");

            }
            else
            {
                return new JsonResult(new { success = false, issue = global_School_Year, errors = ModelState.Values.Where(i => i.Errors.Count > 0) });
            }
        }
  


        [Route("AddBadge")]

        [HttpPost]
        public void AddBadge([FromBody]BadgeDto badgeDto)
        {
            string unique_File_Name = null;
            if(badgeDto.photo!=null)
            {
             String UploadFloder=  Path.Combine(hostingEnvironment.WebRootPath + "uploads");
             unique_File_Name = Guid.NewGuid().ToString() + "_" + badgeDto.photo.FileName;
            string fileName=    Path.Combine(UploadFloder, unique_File_Name);
            badgeDto.photo.CopyTo(new FileStream(fileName, FileMode.Create));
            
            }

        }



    }
    public class BadgeDto
    {
        public string title { get; set; }
        public IFormFile photo { get; set; }
    }


}