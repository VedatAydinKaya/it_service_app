using AutoMapper;
using it_service_app.Data;
using it_service_app.InjectExample;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace it_service_app.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMyDependency _myDependency;
        private readonly MyContext _dbContext;
        private readonly IMapper _mapper;
        public HomeController(IMyDependency myDependency,MyContext dbContext,IMapper mapper)  // constructor injection
        {  
              _myDependency = myDependency;
              _dbContext = dbContext;
             _mapper = mapper;
        }

        public IActionResult Index()
        {
            _myDependency.Log("Home/Index");

            //var data=_dbContext.subscriptionTypes
            //         .ToList()
            //         .Select(x=>_mapper.Map<SubscriptionTypeViewModel>(x)) 
            //         .OrderBy(x=>x.Price)
            //         .ToList();

            return View();
        }
    }
}
