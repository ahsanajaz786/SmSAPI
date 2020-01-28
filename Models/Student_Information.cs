using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Student_Information: BaseEntity
    {
        public int student_Id { get; set; }
        public int tutor_Id { get; set; }
    }
}
