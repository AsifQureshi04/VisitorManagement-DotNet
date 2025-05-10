namespace MFL_VisitorManagement.Dtos;

public class VisitorDetails
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailId { get; set; }
    public string ContactNumber { get; set; }
    public int VisitorId { get; set; }
    public string VisitorPass { get; set; }
    public string Status { get; set; }
    public string VisitDate { get; set; }
    public string InTime { get; set; }
    public string ExitTime { get; set; }
    public string Address      { get; set; }
    public string WhomToMeet   { get; set; }
    public string ReasonToMeet { get; set; }
    public string Department { get; set; }
}
