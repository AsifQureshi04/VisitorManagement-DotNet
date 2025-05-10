using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using MFL_VisitorManagement.Dtos;

namespace MFL_VisitorManagement.Utility
{
    public class Utilities
    {
        public async Task<IActionResult> GetException(string exception, string status)
        {
            return new JsonResult(new
            {
                Message = exception,
                Status = status,
                Token = 0
            }); 
        }

        public static MemoryStream GenerateQRCode(QrCodeData qrData)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(JsonConvert.SerializeObject(qrData), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            using var bitmap = qrCode.GetGraphic(20);

            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            return ms;
        }
    }
}
