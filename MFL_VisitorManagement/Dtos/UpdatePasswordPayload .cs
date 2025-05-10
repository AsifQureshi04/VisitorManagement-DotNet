namespace MFL_VisitorManagement.Dtos;

public class UpdatePasswordPayload
{
    public string UserId { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
