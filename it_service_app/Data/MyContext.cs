﻿
using it_service_app.Models.Entities;
using it_service_app.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace it_service_app.Data
{
    public class MyContext:IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {

        public MyContext(DbContextOptions<MyContext> options) : base(options) 
        {

        }
        public DbSet<Deneme> Denemes  { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionType> subscriptionTypes { get; set; }
    }
}
