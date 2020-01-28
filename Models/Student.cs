using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Student:BaseEntity
    {
        [key]
        public int id { get; set; }
        public string userID { get; set; }
        public string GuardiaID { get; set; }
    }
}
