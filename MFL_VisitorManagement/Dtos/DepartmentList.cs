namespace MFL_VisitorManagement.Dtos;

public class  DepartmentMaster
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public char IsActive { get; set; }
    public char IsDelete { get; set; }
}
