using it_service_app.Models.Payment;
using System.Collections.Generic;

namespace it_service_app.Services
{
    public interface IPaymentService
    {

        public InstallmentModel CheckInstallments(string binNumber, decimal price);

        public PaymentResponseModel Pay(PaymentModel model);
    }
}
