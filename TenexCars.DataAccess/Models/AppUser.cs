using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenexCars.DataAccess.Models
{
    public class AppUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Type { get; set; }


        //public Guid Id { get; set; }

        //[ForeignKey("OperatorId")]
        //public Guid OperatorId { get; set; }
        //public Operator Operator { get; set; }

        //[ForeignKey("SubscriberId")]
        //public Guid SubscriberId { get; set; }
        //public Subscriber Subscriber { get; set; }

    }
}
