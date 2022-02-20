using System;

namespace it_service_app.ViewModels
{
    public class SubscriptionTypeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Month { get; set; }
        public int Price { get; set; }
    }
}
