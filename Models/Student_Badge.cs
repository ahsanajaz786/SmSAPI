using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Student_Badge: BaseEntity
    {
        public int subject_Id { get; set; }
        public int student_Id { get; set; }
        public int badge_Id { get; set; }
        public int year_Id { get; set; }
        public int teacher_Id { get; set; }
    }
}
