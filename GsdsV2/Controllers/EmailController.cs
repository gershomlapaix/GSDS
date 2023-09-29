using GsdsV2.DTO;
using GsdsV2.Services;

namespace GsdsV2.Controllers
{
    public class EmailController
    {
        public static async Task<IResult> sendEmail(EmailDto request, IEmailService emailService)
        {
              emailService.SendEmailAsync(request);
              return TypedResults.Ok(request);
         }
    }
}
