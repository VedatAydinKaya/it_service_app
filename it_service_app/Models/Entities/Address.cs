﻿using it_service_app.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace it_service_app.Models.Entities
{
    public class Address: BaseEntity
    {
        public string Line { get; set; }
        public string PostCode { get; set; }
        public string AddressType { get; set; }

        [StringLength(450)]
        public string UserId { get; set; }
        public int StateId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State  State{ get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

    }

    public enum AddressTypes 
    {
        Fatura,
        Teslimat
    }
}
