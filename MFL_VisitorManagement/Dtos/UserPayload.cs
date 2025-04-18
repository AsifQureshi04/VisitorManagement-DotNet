namespace MFL_VisitorManagement.Dtos
{
    public class UserPayload
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string UserRole { get; set; }
    }
}
