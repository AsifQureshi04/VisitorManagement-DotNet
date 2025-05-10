
namespace MFL_VisitorManagement.Dtos;

public class GetAllVisitorsPayload : PaginationParams
{
    public string? SearchString { get; set; }
    public string? FromDate { get; set; } 
    public string? ToDate { get; set; } 
}
