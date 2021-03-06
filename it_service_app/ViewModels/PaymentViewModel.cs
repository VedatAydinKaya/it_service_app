using it_service_app.Models.Payment;

namespace it_service_app.ViewModels
{
    public class PaymentViewModel
    {

        public CardModel CardModel { get; set; }
        public AddressModel AddressModel { get; set; }
        public BasketModel BasketModel { get; set; }
        public decimal Amount { get; set; } = 1000M;
        public decimal PaidAmount { get; set; }
        public int Installment { get; set; }


    }
}
