using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using MFL_VisitorManagement.Helpers;

namespace MFL_VisitorManagement.Interface
{
    public interface IManageVisitorRepository
    {
        Task<int> AddVisitorRepo(AddVisitorPaylaod addVisitorPaylaod);
        Task<PagedList<VisitorDetails>> GetAllVisitorsRepo(GetAllVisitorsPayload getAllVisitorsPayload);
        Task<bool> UpdateVisitorsRepo(UpdateVisitorPayload updateVisitorPayload);
        Task<IEnumerable<VisitorDetails>> GetVisitorByIdRepo(VisitorById visitorById);

    }
}
