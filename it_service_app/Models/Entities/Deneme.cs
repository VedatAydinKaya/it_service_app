using System.ComponentModel.DataAnnotations;

namespace it_service_app.Models.Entities
{
    public class Deneme
    {
        public int Id { get; set; } 

        [StringLength(20)]
        public string Name { get; set; }
    }
}
