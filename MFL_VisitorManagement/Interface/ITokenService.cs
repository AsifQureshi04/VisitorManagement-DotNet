using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;

namespace MFL_VisitorManagement.Interface
{
    public interface ITokenService
    {
        string CreateToken(UserLoginResponse UserLoginResponse);
    }
}
