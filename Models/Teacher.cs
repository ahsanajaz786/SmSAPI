using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiJwt.Models
{
    public class Teacher
    {
        [Key]
        public int id { get; set; }
        public string userid { get; set; }
     
        public int school_id { get; set; }

    }
    public class TeacherDto
    {
        public string id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        
        public string Password { get; set; }

      
        public String phone { get; set; }
        public IFormFile profileimg { get; set; }
    }
    public class GuardianDto
    {
        public int id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }

        public string Password { get; set; }
        public IFormFile profileimg { get; set; }

        public String phone { get; set; }
     
    }
    public class StudentDto
    {
        public string id { get; set; }
        public int sId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Guardian { get; set; }

        public int ClassSectionID { get; set; }
        public IFormFile Profileimg { get; set; }

        

    }

}
