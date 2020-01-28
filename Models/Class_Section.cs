using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Class_Section: BaseEntity
    {
        public int class_ID { get; set; }
        public int section_id { get; set; }
        //   public string section_Name { get; set; }
        public string advisor { get; set; }
    }
    public class Section : BaseEntity
    {
     
        public string sectionName { get; set; }
      
    }
}
