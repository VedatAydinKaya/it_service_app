using AutoMapper;
using it_service_app.Data;
using it_service_app.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace it_service_app.Components
{
    public class PricingTableViewComponent:ViewComponent
    {
        private readonly MyContext _dbContext;
        private readonly IMapper _mapper;

        public PricingTableViewComponent(MyContext dbContext,Mapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper; 
        }
        public IViewComponentResult Invoke() 
        {
            var data=_dbContext.subscriptionTypes
                .ToList()
                .Select(x=>_mapper.Map<SubscriptionTypeViewModel>(x))
                .OrderBy(x=>x.Price)
                .ToList();

            return View(data);
        }

    }
}

