using Movie.Models;

namespace Movie.Services
{
    public interface IEmailService
    {
        void SendEmail( EmailDTO request);
    }
}
