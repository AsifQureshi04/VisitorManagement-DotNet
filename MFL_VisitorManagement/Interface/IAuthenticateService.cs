using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MFL_VisitorManagement.Interface
{
    public interface IAuthenticateService
    {
        Task<IActionResult> UserLogin(UserPayload UserPayload);
        Task<IActionResult> UserRegistration(User User);
        Task<IActionResult> UpdatePassword(UpdatePasswordPayload UpdatePasswordPayload);

    }
}
