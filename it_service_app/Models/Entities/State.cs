using System.ComponentModel.DataAnnotations.Schema;

namespace it_service_app.Models.Entities
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }


        [ForeignKey("CityId")]
        public virtual City City { get; set; }






    }
}
