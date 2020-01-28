using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiJwt.Entities
{
   
    public class User : IdentityUser
    {
         public string FullName { get; set; }
        
        public bool is_activated { get; set; }
        public int? group_id { get; set; }
        public string Timezone { get; set; }
        public int role_id { get; set; }
        public long space_left { get; set; }
       public string profilePath { get; set; }
        ///public bool PasswordChangeRequired { get; set; }
    }
    public class test : BaseEntity
    {
        public string name { get; set; }
    }

    public class ActivationCode
    {
        [Key]
        public int id { get; set; }

        public string user_id { get; set; }
        public string activationCode { get; set; }
        public DateTime createdDate { get; set; }
    }

    public class Validation_Code
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string validation_code { get; set; }
    }
}
