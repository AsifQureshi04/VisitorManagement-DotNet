using MFL_VisitorManagement.Entities;

namespace MFL_VisitorManagement.Dtos
{
    public class UserRegistration : User
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
