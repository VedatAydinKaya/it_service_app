using Microsoft.AspNetCore.Mvc;

namespace it_service_app.Areas.Admin.Controllers
{
    public class ManageController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
