using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace it_service_app.Models.Entities
{
    public class Subscription:BaseEntity
    {
        public Guid SubscriptionTypeId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime EndDate { get; set; }

        [NotMapped]  // this data-annotation denotes that should be excluded from database mapping
        public bool isActive => EndDate > DateTime.Now;

        [ForeignKey(nameof(SubscriptionTypeId))]
        public virtual SubscriptionType SubscriptionType { get; set; }






    }
}
