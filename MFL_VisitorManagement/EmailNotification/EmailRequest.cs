namespace MFL_VisitorManagement.EmailNotification;

public class EmailRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ToEmail { get; set; }
    public string? VisitorPass { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public Stream QrStream  { get; set; }
    public string ContentId  { get; set; }

}
