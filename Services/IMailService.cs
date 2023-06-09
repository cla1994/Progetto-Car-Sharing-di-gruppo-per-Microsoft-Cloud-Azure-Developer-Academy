using Academy2023.Net.Models;

namespace Academy2023.Net.Services
{
    public interface IMailService
    {
        public Task<bool> SendAsync(MailData mailData, CancellationToken ct);
    }
}