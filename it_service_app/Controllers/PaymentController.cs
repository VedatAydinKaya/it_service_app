using it_service_app.Models.Payment;
using it_service_app.Services;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
                 Installment= paymentViewModel.Installment,
                 Address=paymentViewModel.AddressModel,
                 BasketList=new List<BasketModel>() { paymentViewModel.BasketModel },
                 CardModel=paymentViewModel.CardModel,
                 Customer=new CustomerModel(),
                 Price=1000
                  
            };
            var result = _paymentService.Pay(paymentModel);     
            return View();
        }

    }
}
