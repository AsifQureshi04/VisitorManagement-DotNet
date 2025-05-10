namespace MFL_VisitorManagement.Interface;

public interface IEmailService
{
    Task<IActionResult> SendEmailAsync(EmailRequest request);
    Task<IActionResult> SendEmailWithQrCodeAsync(EmailRequest request);
}
