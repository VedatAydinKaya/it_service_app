using System;
using it_service_app.Data;
using it_service_app.InjectExample;
using it_service_app.Models.Identity;
using it_service_app.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace it_service_app
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //private readonly IConfiguration Configuration;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Password policy
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric=false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;

                // Lockout policy
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.AllowedForNewUsers = false;

                // user policy 
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

            }).AddEntityFrameworkStores<MyContext>().AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings

                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);


                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;


            });
            services.AddTransient<IEmailSender, EmailSender>();   // new services for IEmail Sender modul=>

            services.AddScoped<IMyDependency, NewMyDependency>();

            services.AddControllersWithViews();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();  // adds middleware for redirecting Http request to Https
            app.UseStaticFiles();      //  enables  static file serving for the current request path 

            app.UseRouting();

            app.UseAuthentication(); // adds Authentication which enables authentication enables
            app.UseAuthorization();  // adds  Authorization which enables authorization enables
         

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "default",
                      pattern: "{controller=Home}/{action=Index}/{id?}");

            });



        }
    }
}
