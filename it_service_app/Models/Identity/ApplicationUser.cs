using it_service_app.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace it_service_app.Models.Identity
{
    public class ApplicationUser: IdentityUser
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;


         public virtual List<Address> Addresses { get; set; }

        public virtual List<Subscription> Subscriptions { get; set;}






    }
}
