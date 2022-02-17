using System.ComponentModel.DataAnnotations;

namespace it_service_app.Models.Entities
{
    public class SubscriptionType:BaseEntity
    {
        [Required,StringLength(50)]
        public string Name { get; set; }
        public string Descripiton { get; set; }
        public int  Month { get; set; }

        public decimal Price { get; set; }


    }
}
