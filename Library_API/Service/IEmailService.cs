using Library_API.Helpter;

namespace Library_API.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}
