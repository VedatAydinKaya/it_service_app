using it_service_app.InjectExample;
using Microsoft.AspNetCore.Mvc;
using System;

namespace it_service_app.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMyDependency _myDependency;
        public HomeController(IMyDependency myDependency)  // constructor injection
        {  
              _myDependency = myDependency;
              Console.WriteLine();
        }

        public IActionResult Index()
        {
           // Console.WriteLine();
            _myDependency.Log("Home INDEX Logged In =>>DATE::26.01.22 REST OF MYLIFE");
            return View();
        }
    }
}
