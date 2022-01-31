using it_service_app.Models;
using System.Threading.Tasks;

namespace it_service_app.Services
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage emailMessage);

        
    }
}
