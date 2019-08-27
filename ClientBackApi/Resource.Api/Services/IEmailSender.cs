using SendGrid;
using System.Threading.Tasks;

namespace Resource.Api.Services
{
    public interface IEmailSender
    {
        Task<Response> SendEmail(string email, string toUsername, string subject, string messageHTML);
    }
}
