using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.EmailNotification;
using MFL_VisitorManagement.Interface;
using MFL_VisitorManagement.Service;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;


namespace MFL_VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyVisitorController(IEmailService emailService, ILogger _logger, IManageVisitorService manageVisitorService) : ControllerBase
    {
        [HttpGet("UpdateVisitorRequestStatus")]
        public async Task<IActionResult> UpdateVisitorRequestStatus(string visitorPass, string email, string firstName, string lastName, string status, int visitorId)
        {
            var payload = new UpdateVisitorRequestPayload
            {
                VisitorId = visitorId,
                VisitorPass = visitorPass,
                Status = status,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            return await manageVisitorService.UpdateVisitorRequestStatus(payload);

        }

    }
}
