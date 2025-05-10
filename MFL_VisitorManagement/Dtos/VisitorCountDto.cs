namespace MFL_VisitorManagement.Dtos;

public class VisitorCountDto
{
    public int TodayVisitors { get; set; } = 0;
    public int YesterdayVisitors { get; set; } = 0;
    public int LastWeekVisitors { get; set; } = 0;
    public int TotalVisitors { get; set; } = 0;
}
