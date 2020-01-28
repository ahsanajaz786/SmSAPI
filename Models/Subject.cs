using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Subject: BaseEntity
    {
       public string subject_Name { get; set; }
        public string title { get; set; }
    }
    public class SubjectScore:BaseEntity
    {

        public int subject_Id { get; set; }
        public string teacher_Id { get; set; }

        public string student_Id { get; set; }
        public int todoId { get; set; }
        public bool status { get; set; }
        public double score { get; set; }
    }
}
