namespace MFL_VisitorManagement.Service;

public class AuthenticateService(ILogger _logger, 
                                Utilities utilities,
                                IAuthenticateRepository authenticateRepository,
                                ITokenService _tokenService) : IAuthenticateService
{
    public async Task<IActionResult> UserRegistration(User User)
    {
        _logger.Information("User Registration Service");
        MessageData messageData = new MessageData();

        try
        {
            using var hmac = new HMACSHA512();
            var UserRegistration = new UserRegistration
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                EmailId = User.EmailId,
                UserRole = string.IsNullOrEmpty(User.UserRole) ? "Admin" : User.UserRole,
                PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(User.Password))),
                PasswordSalt = Convert.ToBase64String(hmac.Key)
            };

            string result = await authenticateRepository.UserRegistrationRepo(UserRegistration);

            if (!string.IsNullOrEmpty(result) && result.IndexOf("Error") == -1)
            {
                messageData.Message = "Registered Successfully, you can login now";
                messageData.StatusCode = StatusCodes.Status200OK.ToString();
                messageData.Token = 1;
            }
            else
            {
                messageData.Message = "Registeration failed";
                messageData.StatusCode = "201";
                messageData.Token = 0;
            }

            return new JsonResult(new
            {
                messageData.Message,
                messageData.StatusCode,
                messageData.Token,
            });
        }
        catch (Exception ex)
        {
            _logger.Fatal("User Registration Service");
            return await utilities.GetException(ex.Message, "201");
        }
    }

    public async Task<IActionResult> UserLogin(UserPayload UserPayload)
    {
        _logger.Information("User Login Service");
        MessageData message = new MessageData();
        try
        {
            var result = await authenticateRepository.UserLoginRepo(UserPayload);
            if (result != null)
            {
                message.Data = result!;
                message.StatusCode = StatusCodes.Status200OK.ToString();
                message.Message = "Logged in successfully";
                message.Token = 1;
                message.JwtToken = _tokenService.CreateToken(result);
            }
            else
            {
                message.Data = null!;
                message.StatusCode = StatusCodes.Status200OK.ToString();
                message.Message = "Failed to login";
                message.Token = 0;
            }
        }
        catch (Exception ex)
        {
            _logger.Fatal("User Registration Service");
            return await utilities.GetException(ex.Message, "201");
        }

        return new JsonResult(new
        {
            message.Data,
            message.StatusCode,
            message.Message,
            message.Token,
            message.JwtToken,
        });
    }

    public async Task<IActionResult> UpdatePassword(UpdatePasswordPayload UpdatePasswordPayload)
    {
        _logger.Information("AuthenticateService/UpdatePassword");
        MessageData message = new MessageData();
        try
        {
            var result = await authenticateRepository.UpdatePasswordRepo(UpdatePasswordPayload);
            if (result == 1)
            {
                message.StatusCode = StatusCodes.Status200OK.ToString();
                message.Message = "Password updated successfully";
                message.Token = 1;
            }
            else
            {
                message.StatusCode = StatusCodes.Status200OK.ToString();
                message.Message = "Incorrect UserId or password";
                message.Token = 0;
            }
        }
        catch (Exception ex)
        {
            _logger.Fatal("AuthenticateService/UpdatePassword");
            return await utilities.GetException(ex.Message, "201");
        }

        return new JsonResult(new
        {
            message.Message,
            message.StatusCode,
            message.Token,
        });
    }
}
