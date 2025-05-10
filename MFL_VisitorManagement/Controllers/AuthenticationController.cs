namespace MFL_VisitorManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(ILogger _logger, IAuthenticateService authenticateService) : ControllerBase
{
    [HttpPost("logIn")]
    public async Task<IActionResult> UserLogin(UserPayload UserPayload)
    {
        _logger.Information("AccountController/LogIn");
        return await authenticateService.UserLogin(UserPayload);
    }

    [HttpPost("registration")]
    public async Task<IActionResult> UserRegistration(User user)
    {
        _logger.Information("AccountController/LogIn");
        return await authenticateService.UserRegistration(user);
    }

    [HttpPost("UpdatePassword")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordPayload UpdatePasswordPayload)
    {
        _logger.Information("AccountController/UpdatePassword");
        return await authenticateService.UpdatePassword(UpdatePasswordPayload);
    }
}
