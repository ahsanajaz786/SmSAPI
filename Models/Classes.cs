using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Classes : BaseEntity
    {
        public string level { get; set; }
    }
   
    public class Teacher_Subject : BaseEntity
    {
        public string teacher_Id { get; set; }
        public int subject_Id { get; set; }
        public int class_Id { get; set; }
        public int section_Id { get; set; }
        public int class_section { get; set; }

    }
    public class Student_class : BaseEntity
    {
        public string student_Id { get; set; }
        public int class_Section_Id { get; set; }

    }
    public class School_Year
    {
        [Key]
        public int id { get; set; }
        public int school_ID { get; set; }
        public bool is_deleted { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; } = DateTime.Now;


        public int YearID { get; set; }
        public bool isActive { get; set; }
        /*
        public DateTime start_Date { get; set; }
        public DateTime end_Date { get; set; }*/
    }
    public class Global_School_Year
    {
        [Key]
        public int id { get; set; }
        public bool is_deleted { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; } = DateTime.Now;


        public string year { get; set; }
        public bool isActive { get; set; }

        public DateTime start_Date { get; set; }
        public DateTime end_Date { get; set; }
    }
    public class Global_School_YearDTO
    {
        [Key]
        public int id { get; set; }
        public bool is_deleted { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; } = DateTime.Now;


        public string year { get; set; }
        public bool isActive { get; set; }

        public string start_Date { get; set; }
        public string end_Date { get; set; }
    }
}
