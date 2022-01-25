using Microsoft.AspNetCore.Mvc;

namespace it_service_app.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
