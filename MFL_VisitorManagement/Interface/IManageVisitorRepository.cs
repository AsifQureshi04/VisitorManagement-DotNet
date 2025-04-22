using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using MFL_VisitorManagement.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MFL_VisitorManagement.Interface
{
    public interface IManageVisitorRepository
    {
        Task<int> AddVisitorRepo(AddVisitorPaylaod addVisitorPaylaod);
        Task<PagedList<VisitorDetails>> GetAllVisitorsRepo(GetAllVisitorsPayload getAllVisitorsPayload);
        Task<bool> UpdateVisitorsRepo(UpdateVisitorPayload updateVisitorPayload);
        Task<IEnumerable<VisitorDetails>> GetVisitorByIdRepo(VisitorById visitorById);
        Task<bool> DeleteVisitorByIdRepo(VisitorById visitorById);
        Task<IEnumerable<IdProofMaster>> GetIdProofListRepo();
        Task<IEnumerable<DepartmentMaster>> GetDepartmentListRepo();
        Task<IActionResult> GetVisitorCountRepo(VisitorCountPayload visitorCountPayload);

    }
}
