using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiJwt.Entities;

namespace WebApiJwt.Models
{
    public class TenantInformation: BaseEntity
    {
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public string Timezone { get; set; }
    }
}
