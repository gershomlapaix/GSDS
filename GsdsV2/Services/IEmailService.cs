using GsdsV2.DTO;

namespace GsdsV2.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto request);
    }
}
