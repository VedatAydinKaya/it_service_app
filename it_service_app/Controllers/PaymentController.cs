using AutoMapper;
using it_service_app.Data;
using it_service_app.Extensions;
using it_service_app.Models.Payment;
using it_service_app.Services;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace it_service_app.Controllers
{
    public class PaymentController : Controller
    {

        private readonly IPaymentService _paymentService;
        private readonly MyContext _dbContext;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentservice, MyContext dbContext, IMapper mapper)
        {
            _paymentService = paymentservice;
            _dbContext = dbContext;
            _mapper = mapper;
            var cultureInfo = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        [Authorize]
        public IActionResult Index()

        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CheckInstallment(string binNumber, decimal price)
        {
            if (binNumber.Length != 6 || binNumber.Length > 16)
                return BadRequest(new
                {
                    Message = "Bad req."
                });

            var result = _paymentService.CheckInstallments(binNumber, price);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(PaymentViewModel paymentViewModel)  // 5400360000000003
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
                Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
            };

            var installmentInfo = _paymentService.CheckInstallments(paymentModel.CardModel.CardNumber.Substring(0, 6), paymentModel.Price); // InstallmentModel 

            var installmentNumber = installmentInfo.InstallmentPrices.FirstOrDefault(x => x.InstallmentNumber == paymentViewModel.Installment); // x=>InstallmentPriceModel

            paymentModel.PaidPrice = decimal.Parse(installmentNumber != null ? installmentNumber.TotalPrice.Replace('.', ',') : installmentInfo.InstallmentPrices[0].TotalPrice.Replace('.', ','));

            var result = _paymentService.Pay(paymentModel);

            return View();
        }

        [Authorize]
        public IActionResult Purchase(Guid id)
        {
            var data = _dbContext.subscriptionTypes.Find(id);

            if (data == null)
                return RedirectToAction("Index", "Home");

            var model = _mapper.Map<SubscriptionTypeViewModel>(data);

            ViewBag.Subs = model;

            return View();

        }
        [HttpPost]
        public IActionResult Purchase(PaymentViewModel paymentViewModel)
        {
            var paymentModel = new PaymentModel()
            {
                Installment = paymentViewModel.Installment,
                Address = new AddressModel(),
                BasketList = new List<BasketModel>(),
                Customer = new CustomerModel(),
                CardModel = paymentViewModel.CardModel,
                Price = paymentViewModel.Amount,
                UserId = HttpContext.GetUserId(),
                Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            var installmentInfo = _paymentService.CheckInstallments(paymentModel.CardModel.CardNumber.Substring(0, 6), paymentModel.Price);

            var installmentNumber = installmentInfo.InstallmentPrices.FirstOrDefault(x => x.InstallmentNumber == paymentViewModel.Installment);

            paymentModel.PaidPrice = decimal.Parse(installmentNumber != null ? installmentNumber.TotalPrice : installmentInfo.InstallmentPrices[0].TotalPrice);


            var result = _paymentService.Pay(paymentModel);

            return View();    
        }

    }
}
