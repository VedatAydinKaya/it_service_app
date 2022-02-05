using AutoMapper;
using it_service_app.Models.Payment;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Configuration;
using MUsefulMethods;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace it_service_app.Services
{
    public class IyzicoPaymentService : IPaymentService
    {

        private readonly IyzicoPaymentOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public IyzicoPaymentService(IConfiguration configuration,IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            var section = _configuration.GetSection(IyzicoPaymentOptions.Key);  // Gets a configuration sub-section with the specified key

            _options = new IyzicoPaymentOptions()
            {
                ApiKey = section["ApiKey"],
                SecretKey = section["SecretKey"],
                BaseUrl = section["BaseUrl"],
                ThreedsCallbackUrl =section["ThreedsCallbackUrl"]

            };  
        }
        private string GenerateConverstaionId() 
        {
            return StringHelpers.GenerateUniqueCode();
        }

        public InstallmentModel CheckInstallments(string binNumber, decimal price)
        {
             
            var conversationId=GenerateConverstaionId();

            var request = new RetrieveInstallmentInfoRequest()
            {
                Locale = Locale.TR.ToString(),
                ConversationId = conversationId,
                BinNumber = binNumber,
                Price = price.ToString(new CultureInfo("en-US")),
            };

            var result = InstallmentInfo.Retrieve(request, _options);
            if (result.Status=="failure")
            {
                throw new Exception(result.ErrorMessage);
            }

            if (result.ConversationId!=conversationId)
            {
                throw new Exception("Hatalı Istek Olusturuldu");
            }

            InstallmentModel  resultModel= _mapper.Map<InstallmentModel>(result.InstallmentDetails[0]); // <Tdestination>(object source) = executes mapping from source object to T destination

            Console.WriteLine();
            return null;
        }
        public PaymentResponseModel Pay(PaymentModel model)
        {
            return null;
        }
    }
}
