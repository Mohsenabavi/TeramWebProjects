using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teram.Module.EmailSender.Services.Interfaces;

namespace Teram.Module.EmailSender.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService=emailService??throw new ArgumentNullException(nameof(emailService));
        }

        [HttpPost]
        public bool SendMail(string title, string message, string address)
        {

            var emailSendResult = emailService.SendEmail(title, message, DateTime.Now, address);

            return emailSendResult;

        }
    }
}
