using AutoMapper;
using it_service_app.Models.Entities;
using it_service_app.ViewModels;

namespace it_service_app.MapperProfiles
{
    public class SubscriptionProfile:Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<SubscriptionType, SubscriptionTypeViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
        }
    }
}
