using System;
using System.ComponentModel.DataAnnotations;

namespace it_service_app.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; } // not null

        [StringLength(128)]
        public string CreatedUser { get; set; }

        public DateTime? UpdatedDate { get; set; }  // nullable

        [StringLength(128)]
        public string UpdatedUser { get; set; }
    }
}
