using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.EmailNotification;
using MFL_VisitorManagement.Entities;
using MFL_VisitorManagement.Interface;
using MFL_VisitorManagement.Service;
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
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("idproof-types")]
        public async Task<IActionResult> GetIdProofList()
        {
            _logger.Information("ManageVisitorsController/idproof-types");
            return await manageVisitorService.GetIdProofList();
        }

        [AllowAnonymous]
        [HttpPost("GetDepartmentList")]
        public async Task<IActionResult> GetDepartmentList()
        {
            _logger.Information("ManageVisitorsController/GetDepartmentList");
            return await manageVisitorService.GetDepartmentList();
        }

        [HttpPost("VisitorCount")]
        public async Task<IActionResult> GetVisitorCount()
        {
            _logger.Information("ManageVisitorsController/GetDepartmentList");
            return await manageVisitorService.GetVisitorCount();
        }

        [HttpPost("GetSidebarMenuItems")]
        public async Task<IActionResult> GetMenuItems(RoleIdPayload roleIdPayload)
        {
            _logger.Information("ManageVisitorsController/GetSidebarMenuItems");
            return await manageVisitorService.GetMenuItems(roleIdPayload);
        }

        [HttpPost("UpdateVisitorRequestStatus")]
        public async Task<IActionResult> UpdateVisitorRequestStatus(UpdateVisitorRequestPayload updateVisitorRequestPayload)
        {
            _logger.Information("NotifyVisitorController/AddVisitor");
            return await manageVisitorService.UpdateVisitorRequestStatus(updateVisitorRequestPayload);
        }

        //[HttpPost("addVisitorFormQrCode")]
        //public async Task<IActionResult> GetQrForAddVisitorForm()
        //{
        //    _logger.Information("NotifyVisitorController/AddVisitor");
        //    string FormUrl = "http://localhost:4200/AddVisitor";
        //    return await manageVisitorService.GenerateQrCode(FormUrl,null);
        //}

        [AllowAnonymous]
        [HttpPost("CheckIfVisitorExistsByEmail")]
        public async Task<IActionResult> CheckIfVisitorExists(VisitorPass_EmailPayload visitorPass_EmailPayload)
        {
            _logger.Information("NotifyVisitorController/CheckIfVisitorExistsByEmail");
            return await manageVisitorService.CheckIfVisitorExists(visitorPass_EmailPayload);
        }


    }
}
