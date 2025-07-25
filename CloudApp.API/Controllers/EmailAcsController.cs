using CloudApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailTestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendTestEmail()
        {
            await _emailService.SendEmailAsync("shubhichourey010@gmail.com", "Test from ACS", "<h2>Hello from Azure!</h2>");
            return Ok("Email sent.");
        }
    }

}
