using it_service_app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
