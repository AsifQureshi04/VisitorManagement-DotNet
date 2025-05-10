namespace MFL_VisitorManagement.Extensions;

public static class AddIdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    
        })
        .AddJwtBearer(options =>
            {
                var tokenKey = configuration["TokenKey"] ?? throw new Exception("Token key not found");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };
            });
        return services;
    }

    public static string GenerateVisitorRequestQrCode(this IServiceCollection services)
    {
        string url = "http://192.168.0.169:4200/VisitorRequest";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCode(qrCodeData);
        using var bitmap = qrCode.GetGraphic(20);

        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Png);
        var base64 = Convert.ToBase64String(ms.ToArray());

        var code =  $"data:image/png;base64,{base64}";
        return code;
    }
}
