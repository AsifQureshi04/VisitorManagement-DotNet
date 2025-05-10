using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MFL_VisitorManagement.EmailNotification;
using MFL_VisitorManagement.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net.Mime;
using static QRCoder.PayloadGenerator;
using System.Net;


namespace MFL_VisitorManagement.Service
{
    public class EmailService(IOptions<EmailSettings> options) : IEmailService
    {
        public async Task<IActionResult> SendEmailAsync(EmailRequest request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(options.Value.SenderName, options.Value.SenderEmail));
                email.To.Add(MailboxAddress.Parse(request.ToEmail));
                email.Subject = request.Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = request.Body
                };

                email.Body = builder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                var secureSocketOption = options.Value.Port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

                await smtp.ConnectAsync(options.Value.SmtpServer, options.Value.Port, secureSocketOption);
                await smtp.AuthenticateAsync(options.Value.Username, options.Value.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return new JsonResult(new
                {
                    Message = "Email sent successfully",
                    Token = 1,
                    StatusCode = StatusCodes.Status200OK.ToString()
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Message = $"Failed to send email: {ex.Message}",
                    Token = 0,
                    StatusCode = StatusCodes.Status500InternalServerError.ToString()
                });
            }
        }

        public async Task<IActionResult> SendEmailWithQrCodeAsync(EmailRequest request)
        {
            try
            {
                var fromAddress = new MailAddress(options.Value.SenderEmail, options.Value.SenderName);
                var toAddress = new MailAddress(request.ToEmail!);
                string fromPassword = options.Value.Password;

                var smtp = new System.Net.Mail.SmtpClient
                {
                    Host = options.Value.SmtpServer, 
                    Port = options.Value.Port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = request.Subject,
                    IsBodyHtml = true
                };

                //string contentId = "qrcode123";
                string htmlBody = request.Body!;


                var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

                var qrImage = new LinkedResource(request.QrStream, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = request.ContentId,
                    TransferEncoding = TransferEncoding.Base64,
                    ContentType = new System.Net.Mime.ContentType("image/jpeg")
                };

                htmlView.LinkedResources.Add(qrImage);
                message.AlternateViews.Add(htmlView);

                smtp.Send(message);

                return new JsonResult(new
                {
                    Message = "Email sent successfully",
                    Token = 1,
                    StatusCode = StatusCodes.Status200OK.ToString()
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Message = $"Failed to send email: {ex.Message}",
                    Token = 0,
                    StatusCode = StatusCodes.Status500InternalServerError.ToString()
                });
            }
        }
    }
}
