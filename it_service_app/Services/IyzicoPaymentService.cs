using AutoMapper;
using it_service_app.Models.Identity;
using it_service_app.Models.Payment;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public IyzicoPaymentService(IConfiguration configuration, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            var section = _configuration.GetSection(IyzicoPaymentOptions.Key);  // Gets a configuration sub-section with the specified key

            _options = new IyzicoPaymentOptions()
            {
                ApiKey = section["ApiKey"],
                SecretKey = section["SecretKey"],
                BaseUrl = section["BaseUrl"],
                ThreedsCallbackUrl = section["ThreedsCallbackUrl"]

            };
        }
        private string GenerateConverstaionId()
        {
            return StringHelpers.GenerateUniqueCode();
        }

        private CreatePaymentRequest InitialPaymentRequest(PaymentModel paymentModel)
        {

            var paymentRequest = new CreatePaymentRequest()
            {

                Installment = paymentModel.Installment,
                Locale = Locale.TR.ToString(),
                ConversationId = GenerateConverstaionId(),
                Price = paymentModel.Price.ToString(new CultureInfo("en-US")),
                PaidPrice = paymentModel.PaidPrice.ToString(new CultureInfo("en-US")),
                Currency = Currency.TRY.ToString(),
                BasketId = GenerateConverstaionId(),
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.SUBSCRIPTION.ToString(),    
            };

            paymentRequest.PaymentCard = _mapper.Map<PaymentCard>(paymentModel.CardModel);  // creates mapping confiugaration from  Cardmodel to PaymentCard mapping configuration dobe

            var user = _userManager.FindByIdAsync(paymentModel.UserId).Result;

            var buyer = new Buyer()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                GsmNumber = user.PhoneNumber,
                Email =user.Email,
                IdentityNumber ="11111111110",
                LastLoginDate = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RegistrationDate =$"{user.CreatedDate:yyyy-MM-dd HH:mm:ss}",
                RegistrationAddress = "Cihannuma Mah. Barbaros Bulvarı No:9 Beşiktaş",
                Ip = paymentModel.Ip,
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "3473"

            };

            paymentRequest.Buyer = buyer;

            var  billingAdress = new Address()
            {
                ContactName =$"{user.Name} {user.Surname}",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Cihannuma Mah. Barbaros Bulvarı No:9 Beşiktaş",
                ZipCode = "3473"
            };

            paymentRequest.BillingAddress= billingAdress;


            var basketItems = new List<BasketItem>();

            var firstBasketItem = new BasketItem()
            {
                Id = "BI101",
                Name = "Binocular",
                Category1 = "Collectibles",
                Category2 = "Accessories",
                ItemType = BasketItemType.VIRTUAL.ToString(),  // not required for ShippingAdress
                Price = paymentModel.Price.ToString(),
            };
            basketItems.Add(firstBasketItem);

            paymentRequest.BasketItems = basketItems;

            return paymentRequest;
        }
        public InstallmentModel CheckInstallments(string binNumber, decimal price)
        {
             
            var conversationId=GenerateConverstaionId();

            var request = new RetrieveInstallmentInfoRequest()
            {
                Locale = Locale.TR.ToString(),
                ConversationId = conversationId,
                BinNumber = binNumber.Substring(0,6),
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

            var  resultModel= _mapper.Map<InstallmentModel>(result.InstallmentDetails[0]); // <Tdestination>(object source) = executes mapping from source object to T destination

            Console.WriteLine();
            return resultModel;
        }
        public PaymentResponseModel Pay(PaymentModel paymentModel)
        {
            var request = this.InitialPaymentRequest(paymentModel); // return CreatePaymentRequest
            var payment = Payment.Create(request, _options);

            return _mapper.Map<PaymentResponseModel>(payment);
        }
    }
}
