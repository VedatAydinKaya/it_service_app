using System.Diagnostics;

namespace it_service_app.InjectExample
{
    public class MyDependency : IMyDependency
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
