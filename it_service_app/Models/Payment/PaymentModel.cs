using System.Collections.Generic;

namespace it_service_app.Models.Payment
{
    public class PaymentModel
    {
        public string PaymentId { get; set; }
        public decimal Price { get; set; } = 1000M;
        public decimal PaidPrice { get; set; }
        public int Installment { get; set; }
        public CardModel CardModel { get; set; }
        public List<BasketModel> BasketList { get; set; }
        public CustomerModel Customer { get; set; }
        public AddressModel Address { get; set; }
        public string Ip { get; set; }
        public string UserId { get; set; }

    }
}
