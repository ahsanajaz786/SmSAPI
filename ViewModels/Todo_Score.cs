using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiJwt.ViewModels
{
    public class Save_Todo_Score
    {
        public string student_id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int subject_id { get; set; }
        public int todoid { get; set; }
        public string score { get; set; }
    }
    public class Get_Section_Subject
    {
        public string teacher_ID { get; set; }
        public int section_ID { get; set; }
        public string title { get; set; }
        public int subject_id { get; set; }

        public string name { get; set; }

    }
}
