using System;
using System.Diagnostics;

namespace it_service_app.InjectExample
{
    public class NewMyDependency : IMyDependency
    {
        public void Log(string message)
        {
            Debug.WriteLine($"{DateTime.Now:T}-{message}");
        }
    }
}
