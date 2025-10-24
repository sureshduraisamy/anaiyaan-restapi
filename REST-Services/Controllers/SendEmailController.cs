using AutomatedEmailSender;
using Microsoft.AspNetCore.Mvc;
using System;

namespace REST_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] EmailRequest request)
        {
            try
            {
                var emailService = new BuiltinEmailService(
                    request.FromAddress,
                    request.ToAddress,
                    request.CcAddress,
                    request.BccAddress,
                    request.Subject,
                    request.Content,
                    request.GmailAppPassword
                );

                emailService.SendEmail();

                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }
    }
}
