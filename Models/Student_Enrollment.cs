using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Student_Enrollment: BaseEntity
    {
        public int class_Id { get; set; }
        public int section_Id { get; set; }
        public int student_Id { get; set; }
        public int year_Id { get; set; }
    }
}
