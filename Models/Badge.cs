using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class Badge: BaseEntity
    {
        public string title { get; set; }
        public string image_Path { get; set; }
        

    }
}
