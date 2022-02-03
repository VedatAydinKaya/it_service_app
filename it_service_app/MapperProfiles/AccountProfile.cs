using AutoMapper;
using it_service_app.Models.Identity;
using it_service_app.ViewModels;

namespace it_service_app.MapperProfiles
{
    public class AccountProfile:Profile
    {
        public AccountProfile()
        {
            CreateMap<ApplicationUser,UserProfileViewModel>().ReverseMap(); // creates a mapping from UserProfileViewModel                                                            (sourceaobject) to destination object ApplicationUser
        }
    }
}
