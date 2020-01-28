using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Todo: BaseEntity
    {
        public string title { get; set; }
        public String description { get; set; }
        public DateTime date_Time { get; set; }

       
       
        public Todo_Type.type type { get; set; }
        public double score { get; set; }
        public int subject_Id { get; set; }
        public string teacher_Id { get; set; }
        public int class_Id { get; set; }
      
        public int section_Id { get; set; }
        public String fileName { get; set; }
        public String fileUploadPath { get; set; }

    }
}
