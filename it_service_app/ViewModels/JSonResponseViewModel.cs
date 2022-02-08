using System;

namespace it_service_app.ViewModels
{
    public class JSonResponseViewModel
    {
        public bool IsSuccess { get; set; } = true;

        public object Data { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime ResponseTime { get; set; }=DateTime.UtcNow;
    }
}
