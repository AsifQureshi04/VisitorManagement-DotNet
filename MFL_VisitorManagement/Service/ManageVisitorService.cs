using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using MFL_VisitorManagement.Helpers;
using MFL_VisitorManagement.Interface;
using MFL_VisitorManagement.ResponseModel;
using MFL_VisitorManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;


namespace MFL_VisitorManagement.Service
{
    public class ManageVisitorService(ILogger logger, 
                                      IManageVisitorRepository manageVisitorRepository,
                                      Utilities utilities) : IManageVisitorService
    {
        public async Task<IActionResult> AddVisitor(AddVisitorPaylaod addVisitorPaylaod)
        {
            logger.Information("ManageVisitorService/AddVisitor");
            MessageData data = new MessageData();
            try
            {
                var result = await manageVisitorRepository.AddVisitorRepo(addVisitorPaylaod);
                if(result == -1)
                {
                    data.Message = "Visitor added successfully";
                    data.StatusCode = StatusCodes.Status200OK.ToString();
                    data.Token = 1;
                }
                else
                {
                    data.Message = "There is some issue while adding visitor";
                    data.StatusCode = StatusCodes.Status200OK.ToString();
                    data.Token = 0;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("Exception from ManageVisitorService/AddVisitor");
                return await utilities.GetException(ex.Message, "201");
            }

            return new JsonResult(new
            {
                data.Message,
                data.StatusCode,
                data.Token
            });
        }
        public async Task<IActionResult> GetAllVisitors(GetAllVisitorsPayload getAllVisitorsPayload)
        {
            logger.Information("ManageVisitorService/GetAllVisitors");
            MessageData data = new MessageData();
            try
            {
                var result = await manageVisitorRepository.GetAllVisitorsRepo(getAllVisitorsPayload);
                if (result.Count > 0)
                {
                    data.Data = result;
                    data.Data1 = new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages)
                                {
                                   CurrentPage  = result.CurrentPage ,
                                   ItemsPerPage = result.PageSize,
                                   TotalItems  =  result.TotalCount,
                                   TotalPages  =  result.TotalPages
                    };

                    data.Message = "Data fetched successfully";
                    data.StatusCode = StatusCodes.Status200OK.ToString();
                    data.Token = 1;
                }
                else
                {
                    data.Message = "No data found";
                    data.StatusCode = StatusCodes.Status200OK.ToString();
                    data.Token = 0;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("Exception from ManageVisitorService/GetAllVisitors");
                return await utilities.GetException(ex.Message, "201");
            }

            return new JsonResult(new
            {
                data.Data,
                data.Data1,
                data.Message,
                data.StatusCode,
                data.Token
            });
        }
        public async Task<IActionResult> UpdateVisitors(UpdateVisitorPayload updateVisitorPayload)
        {
            logger.Information("ManageVisitorService/UpdateVisitors");
            MessageData data = new MessageData();
            try
            {
                var result = await manageVisitorRepository.UpdateVisitorsRepo(updateVisitorPayload);
                if (result)
                {
                    data.Message = "Visitor updated successfully";
                    data.StatusCode = StatusCodes.Status200OK.ToString();
                    data.Token = 1;
                }
                else
                {
                    data.Message = "Visitor Not updated";
                    data.StatusCode = StatusCodes.Status200OK.ToString();
                    data.Token = 0;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("Exception from ManageVisitorService/UpdateVisitors");
                return await utilities.GetException(ex.Message, "201");
            }

            return new JsonResult(new
            {
                data.Message,
                data.StatusCode,
                data.Token
            });
        }
        public async Task<IActionResult> GetVisitorById(VisitorById visitorById)
        {
            logger.Information("ManageVisitorService/GetVisitorById");
            MessageData data = new MessageData();
            try
            {
                var result = await manageVisitorRepository.GetVisitorByIdRepo(visitorById);
                if (result != null)
                {
                    data.Data = result.FirstOrDefault()!;
                    data.Message = "Data fetched successfully";
                    data.StatusCode = StatusCodes.Status200OK.ToString();
                    data.Token = 1;
                }
                else
                {
                    data.Message = "No data found";
                    data.StatusCode = StatusCodes.Status200OK.ToString();
                    data.Token = 0;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("Exception from ManageVisitorService/GetVisitorById");
                return await utilities.GetException(ex.Message, "201");
            }

            return new JsonResult(new
            {
                data.Data,
                data.Message,
                data.StatusCode,
                data.Token
            });
        }   
    }
}
