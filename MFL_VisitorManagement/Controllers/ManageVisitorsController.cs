using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using MFL_VisitorManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;


namespace MFL_VisitorManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ManageVisitorsController(ILogger _logger, IManageVisitorService manageVisitorService) : ControllerBase
    {
        [HttpPost("AddVisitor")]
        public async Task<IActionResult> AddVisitor(AddVisitorPaylaod addVisitorPaylaod)
        {
            _logger.Information("ManageVisitorsController/AddVisitor");
            return await manageVisitorService.AddVisitor(addVisitorPaylaod);
        }

        [HttpPost("GetAllVisitors")]
        public async Task<IActionResult> GetAllVisitors(GetAllVisitorsPayload? getAllVisitorsPayload)
        {
            _logger.Information("ManageVisitorsController/GetAllVisitors");
            return await manageVisitorService.GetAllVisitors(getAllVisitorsPayload!);
        }

        [HttpPost("UpdateVisitors")]
        public async Task<IActionResult> UpdateVisitors(UpdateVisitorPayload updateVisitorPayload)
        {
            _logger.Information("ManageVisitorsController/UpdateVisitors");
            return await manageVisitorService.UpdateVisitors(updateVisitorPayload!);
        }

        [HttpPost("GetVisitorById")]
        public async Task<IActionResult> GetVisitorById(VisitorById visitorById)
        {
            _logger.Information("ManageVisitorsController/UpdateVisitors");
            return await manageVisitorService.GetVisitorById(visitorById!);
        }

        [HttpPost("DeleteVisitorById")]
        public async Task<IActionResult> DeleteVisitorById(VisitorById visitorById)
        {
            _logger.Information("ManageVisitorsController/DeleteVisitorById");
            return await manageVisitorService.DeleteVisitorById(visitorById!);
        }

        [HttpPost("idproof-types")]
        public async Task<IActionResult> GetIdProofList()
        {
            _logger.Information("ManageVisitorsController/idproof-types");
            return await manageVisitorService.GetIdProofList();
        }

        [HttpPost("GetDepartmentList")]
        public async Task<IActionResult> GetDepartmentList()
        {
            _logger.Information("ManageVisitorsController/GetDepartmentList");
            return await manageVisitorService.GetDepartmentList();
        }

        [HttpPost("VisitorCount")]
        public async Task<IActionResult> GetVisitorCount(VisitorCountPayload visitorCountPayload)
        {
            _logger.Information("ManageVisitorsController/GetDepartmentList");
            return await manageVisitorService.GetVisitorCount(visitorCountPayload);
        }
    }
}
