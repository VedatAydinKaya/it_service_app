using it_service_app.Models.Payment;

namespace it_service_app.Services
{
    public interface IPaymentService
    {
        public void Pay(PaymentModel model);
    }
}
