using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MFL_VisitorManagement.Interface
{
    public interface IManageVisitorService
    {
        Task<IActionResult> AddVisitor(AddVisitorPaylaod addVisitorPaylaod);
        Task<IActionResult> GetAllVisitors(GetAllVisitorsPayload getAllVisitorsPayload);
        Task<IActionResult> UpdateVisitors(UpdateVisitorPayload updateVisitorPayload);
        Task<IActionResult> GetVisitorById(VisitorById visitorById);
        Task<IActionResult> DeleteVisitorById(VisitorById visitorById);
        Task<IActionResult> GetIdProofList();
        Task<IActionResult> GetDepartmentList();
        Task<IActionResult> GetVisitorCount(VisitorCountPayload visitorCountPayload);
    }
}
