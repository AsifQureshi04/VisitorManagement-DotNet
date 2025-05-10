namespace MFL_VisitorManagement.Interface;

public interface IManageVisitorRepository
{
    Task<string> AddVisitorRepo(AddVisitorPaylaod addVisitorPaylaod);
    Task<PagedList<VisitorDetails>> GetAllVisitorsRepo(GetAllVisitorsPayload getAllVisitorsPayload);
    Task<bool> UpdateVisitorsRepo(UpdateVisitorPayload updateVisitorPayload);
    Task<IEnumerable<VisitorDetails>> GetVisitorByIdRepo(VisitorById visitorById);
    Task<bool> DeleteVisitorByIdRepo(VisitorById visitorById);
    Task<IEnumerable<IdProofMaster>> GetIdProofListRepo();
    Task<IEnumerable<DepartmentMaster>> GetDepartmentListRepo();
    Task<IEnumerable<VisitorCountDto>> GetVisitorCountRepo();
    Task<IEnumerable<MenuItem>> GetMenuItemsRepo(RoleIdPayload roleIdPayload);
    Task<int> UpdateVisitorRequestStatusRepo(UpdateVisitorRequestPayload updateVisitorRequestPayload);
    Task<(int Result, string VisitingOfficialEmail, string FirstName, string LastName,string VisitingOfficialName, int VisitorId)> CheckIfVisitorExistsRepo(VisitorPass_EmailPayload visitorPass_EmailPayload);
    //Task<IEnumerable<MenuItem>> GetVisitorId_VisitingOfficialEmail(RoleIdPayload roleIdPayload);


}
