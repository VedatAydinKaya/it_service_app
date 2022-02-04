using it_service_app.Models.Payment;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace it_service_app.Services
{
    public class IyzicoPaymentService : IPaymentService
    {

        private readonly IyzicoPaymentOptions _options;
        private readonly IConfiguration _configuration;
        public IyzicoPaymentService(IConfiguration configuration)
        {
            _configuration = configuration;

            var section = _configuration.GetSection(IyzicoPaymentOptions.Key);  // Gets a configuration sub-section with the specified key

            _options = new IyzicoPaymentOptions()
            {
                ApiKey = section["ApiKey"],
                SecretKey = section["SecretKey"],
                BaseUrl = section["BaseUrl"],
                ThreedsCallbackUrl =section["ThreedsCallbackUrl"]

            };
            System.Console.WriteLine();
        }

        public List<InstallmentModel> CheckInstallments(string binNumber, decimal price)
        {
            return null;
        }

        public PaymentResponseModel Pay(PaymentModel model)
        {
            return null;
        }
    }
}
