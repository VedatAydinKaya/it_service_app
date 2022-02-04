using it_service_app.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace it_service_app.Models.Entities
{
    public class Subscription : BaseEntity
    {
        public Guid SubscriptionTypeId { get; set; }
        public decimal Amount { get; set; }  // not null 
        public decimal PaidAmount { get; set; }  // not null
        public DateTime EndDate { get; set; }   // not null

        [NotMapped]  // this data-annotation denotes that should be excluded from database mapping
        public bool isActive => EndDate > DateTime.Now;

        [ForeignKey(nameof(SubscriptionTypeId))]
        public virtual SubscriptionType SubscriptionType { get; set; }


        [StringLength(450)]
        public string UserId { get; set; }



        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }






    }
}
