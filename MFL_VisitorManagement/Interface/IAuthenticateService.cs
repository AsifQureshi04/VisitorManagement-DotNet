namespace MFL_VisitorManagement.Interface;

public interface IAuthenticateService
{
    Task<IActionResult> UserLogin(UserPayload UserPayload);
    Task<IActionResult> UserRegistration(User User);
    Task<IActionResult> UpdatePassword(UpdatePasswordPayload UpdatePasswordPayload);

}
