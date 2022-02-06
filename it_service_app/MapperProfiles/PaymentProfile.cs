using AutoMapper;
using it_service_app.Models.Payment;
using Iyzipay.Model;

namespace it_service_app.MapperProfiles
{
    public class PaymentProfile:Profile
    {
        public PaymentProfile()
        {
            CreateMap<CardModel, PaymentCard>(); // this allows for two-way mapping
            CreateMap<BasketModel, BasketItem>().ReverseMap();
            CreateMap<AddressModel, Address>().ReverseMap();
            CreateMap<CustomerModel, Buyer>().ReverseMap();
            CreateMap<InstallmentPriceModel, InstallmentPrice>().ReverseMap();
            CreateMap<InstallmentModel, InstallmentDetail>().ReverseMap();
            CreateMap<PaymentResponseModel,Payment>().ReverseMap();  
        }
    }
}
