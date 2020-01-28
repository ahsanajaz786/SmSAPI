using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiJwt.Models
{
    public class Todo_Type
    {
      public  enum type{
           // [EnumMember(Value = "Short Quiz")]
            [Description("Short Quiz")]
            ShortQuiz,
            [Description("Long Quiz")]

            LongQuiz,
            [Description("Assignment")]
            Assignment,
            [Description("Recitation")]
            Recitation,
            [Description("Exam")]
            Exam,
           
            [Description("Project")]

            Project,
            [Description("Surprize Test")]


            SurprizeTest,
           [Description("Reminder")]


                 Reminder,
            [Description("Personal Task")]

            PersonalTask

        }
    }
}
