namespace MFL_VisitorManagement.ResponseModel;

public class MessageData
{
    public string Message { get; set; }
    public object Data { get; set; }
    public object? Data1 { get; set; }
    public string StatusCode { get; set; }
    public int Token { get; set; }
    public string JwtToken { get; set; }
    public string UserId { get; set; }
}
