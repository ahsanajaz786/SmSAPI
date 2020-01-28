using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Event:BaseEntity
    {
        public string title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string teacher_id { get; set; }

        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }

    }
    public class EventDto
    {
        public int id { get; set; }

        public string title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string teacher_id { get; set; }

        public string startDateTime { get; set; }
        public string endDateTime { get; set; }
        public List<int> clasid { get; set; }
        public IFormFile uploadFile { get; set; }

    }
   
    public class ClassEvent : BaseEntity
    {
        public int clasid { get; set; }
        public int eventID { get; set; }
    

    }


    public class EventAttachment : BaseEntity
    {
        public int event_id { get; set; }
        public string file_name { get; set; }
        public string file_path{ get; set; }
        public string file_type { get; set; }
        public string mtype { get; set; }
    }


    public class EventAtachmentUpload
    {
        public int event_id { get; set; }
        public IFormFile upload { get; set; }

    }
}
