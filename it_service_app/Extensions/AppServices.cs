using it_service_app.InjectExample;
using it_service_app.MapperProfiles;
using it_service_app.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace it_service_app.Extensions
{
    public static class AppServices
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(options =>
            {
                options.AddProfile(typeof(PaymentProfile));
                options.AddProfile(typeof(AccountProfile));
                options.AddProfile<SubscriptionProfile>();
               
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IPaymentService, IyzicoPaymentService>(); 
            services.AddScoped<IMyDependency, NewMyDependency>();

            return services;
        }
        



    }
}
