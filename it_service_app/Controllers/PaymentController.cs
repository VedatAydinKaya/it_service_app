using it_service_app.Extensions;
using it_service_app.Models.Payment;
using it_service_app.Services;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace it_service_app.Controllers
{
    public class PaymentController : Controller
    {

        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentservice)
        {
            _paymentService = paymentservice;
        }

        [Authorize]
        public IActionResult Index()
        
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CheckInstallment(string binNumber) 
        {
            if (binNumber.Length != 6)
                return BadRequest(new
                {
                    Message = "Bad req."
                });

            var result = _paymentService.CheckInstallments(binNumber, 1000);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(PaymentViewModel paymentViewModel)
        {
            var paymentModel = new PaymentModel()
            {
                Installment = paymentViewModel.Installment,
                Address = paymentViewModel.AddressModel,
                BasketList = new List<BasketModel>(),
                CardModel = paymentViewModel.CardModel,
                Customer = new CustomerModel(),
                Price = 1000,
                UserId = HttpContext.GetUserId(),
                Ip=Request.HttpContext.Connection.RemoteIpAddress?.ToString(),                  
            };

            var installmentInfo = _paymentService.CheckInstallments(paymentModel.CardModel.CardNumber.Substring(0, 6), paymentModel.Price);

            var installmentNumber = installmentInfo.InstallmentPrices.FirstOrDefault(x => x.InstallmentNumber == paymentViewModel.Installment);

            if (installmentNumber!=null)
            {
                paymentModel.PaidPrice = decimal.Parse(installmentNumber.TotalPrice);
            }
            else
            {
                paymentModel.PaidPrice = decimal.Parse(installmentInfo.InstallmentPrices[0].TotalPrice);
            }


            var result = _paymentService.Pay(paymentModel);     
            return View();
        }

    }
}
