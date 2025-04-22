using MFL_VisitorManagement.Data;
using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using MFL_VisitorManagement.Helpers;
using MFL_VisitorManagement.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading;

namespace MFL_VisitorManagement.Repositories
{
    public class ManageVisitorRepository(DataContext context) : IManageVisitorRepository
    {
        public async Task<int> AddVisitorRepo(AddVisitorPaylaod addVisitorPaylaod)
        {
          
            var parameters = new[]
            {
                new OracleParameter("p_FirstName", addVisitorPaylaod.FirstName),
                new OracleParameter("p_LastName", addVisitorPaylaod.LastName ?? (object)DBNull.Value),
                new OracleParameter("p_Email", addVisitorPaylaod.Email),
                new OracleParameter("p_PhoneNumber", addVisitorPaylaod.PhoneNumber),
                new OracleParameter("p_Address", addVisitorPaylaod.Address),
                new OracleParameter("p_WhomToMeet", addVisitorPaylaod.WhomToMeet),
                new OracleParameter("p_Department", addVisitorPaylaod.Department),
                new OracleParameter("p_IdProof", addVisitorPaylaod.IdProof),
                new OracleParameter("p_IdProofNumber", addVisitorPaylaod.IdProofNumber),
                new OracleParameter("p_ReasonToMeet", addVisitorPaylaod.ReasonToMeet),
                new OracleParameter("p_VisitDate", OracleDbType.Date)
                {
                    Value = addVisitorPaylaod.VisitDate.Date
                },
                new OracleParameter("p_InTime", OracleDbType.TimeStamp)
                {
                    Value = DateTime.Today.Add(addVisitorPaylaod.InTime.ToTimeSpan())
                }
            };

            var res =  await context.Database.ExecuteSqlRawAsync(
                @"BEGIN Sp_AddVisitor(
            :p_FirstName, :p_LastName, :p_Email, :p_PhoneNumber,
            :p_Address, :p_WhomToMeet, :p_Department,
            :p_IdProof, :p_IdProofNumber, :p_ReasonToMeet,
            :p_VisitDate, :p_InTime); END;", parameters);

            return res;
        }
        public async Task<PagedList<VisitorDetails>> GetAllVisitorsRepo(GetAllVisitorsPayload getAllVisitorsPayload)
        {
            var parameters = new[]
            {
                new OracleParameter("p_VisitorId", OracleDbType.Varchar2)
                {
                    Value = null
                },
                new OracleParameter("p_SearchString", OracleDbType.Varchar2)
                {
                    Value = getAllVisitorsPayload.SearchString ?? string.Empty
                },
                new OracleParameter("p_fromDate", OracleDbType.Varchar2)
                {
                    Value = getAllVisitorsPayload.FromDate
                },
                new OracleParameter("p_toDate", OracleDbType.Varchar2)
                {
                    Value = getAllVisitorsPayload.ToDate
                },
                new OracleParameter("o_cursor", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                }
            };

            using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "Sp_GetAllVisitors";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
                
            await context.Database.OpenConnectionAsync();

            var reader = await command.ExecuteReaderAsync();

            var result = new List<VisitorDetails>();
            while (await reader.ReadAsync())
            {
                
                result.Add(new VisitorDetails
                {
                    VisitorId = reader["VisitorId"] != DBNull.Value ? Convert.ToInt32(reader["VisitorId"]) : 0,
                    FirstName = reader["FirstName"]?.ToString()!,
                    LastName = reader["LastName"]?.ToString()!,
                    EmailId = reader["EmailId"]?.ToString()!,
                    ContactNumber = reader["ContactNumber"]?.ToString()!,
                    VisitDate = reader["VisitDate"]?.ToString()!,
                    VisitorPass = reader["VisitorPass"]?.ToString()!,
                    InTime = reader["InTime"]?.ToString()!,
                    ExitTime = reader["ExitTime"]?.ToString()!,
                    Address = reader["Address"]?.ToString()!,
                    WhomToMeet = reader["WhomToMeet"]?.ToString()!,
                    ReasonToMeet = reader["ReasonToMeet"]?.ToString()!,
                    Department = reader["Department"]?.ToString()!
                });
            }

            await reader.CloseAsync();
            await context.Database.CloseConnectionAsync();
            
            return await PagedList<VisitorDetails>.CreateAsync(result, getAllVisitorsPayload.PageNumber,getAllVisitorsPayload.PageSize);
            

        }
        public async Task<bool> UpdateVisitorsRepo(UpdateVisitorPayload updateVisitorPayload)
        {
            var parameters = new[]
            {
                new OracleParameter("p_VisitorId", OracleDbType.Int32)
                {
                    Value = updateVisitorPayload.VisitorId
                },
                new OracleParameter("p_ExitTime", OracleDbType.Varchar2)
                {
                    Value = updateVisitorPayload.ExitTime
                },
                new OracleParameter("p_IsUpdated", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                }
            };

            await context.Database.ExecuteSqlRawAsync(
                @"BEGIN Sp_UpdateVisitor(:p_VisitorId, :p_ExitTime, :p_IsUpdated); END;",
                parameters
            );

            var result = (Oracle.ManagedDataAccess.Types.OracleDecimal)parameters[2].Value;
            return result.ToInt32() == 1;
        }
        public async Task<IEnumerable<VisitorDetails>> GetVisitorByIdRepo(VisitorById visitorById)
        {
            var parameters = new[]
            {
                new OracleParameter("p_VisitorId", OracleDbType.Varchar2)
                {
                    Value = visitorById.VisitorId
                },
                new OracleParameter("p_SearchString", OracleDbType.Varchar2)
                {
                    Value = string.Empty
                },
                new OracleParameter("p_fromDate", OracleDbType.Varchar2)
                {
                    Value = string.Empty,   
                },
                new OracleParameter("p_toDate", OracleDbType.Varchar2)
                {
                    Value = string.Empty
                },
                new OracleParameter("o_cursor", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                }
            };

            using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "Sp_GetAllVisitors";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await context.Database.OpenConnectionAsync();

            var reader = await command.ExecuteReaderAsync();

            var result = new List<VisitorDetails>();
            while (await reader.ReadAsync())
            {

                result.Add(new VisitorDetails
                {
                    VisitorId = reader["VisitorId"] != DBNull.Value ? Convert.ToInt32(reader["VisitorId"]) : 0,
                    FirstName = reader["FirstName"]?.ToString()!,
                    LastName = reader["LastName"]?.ToString()!,
                    EmailId = reader["EmailId"]?.ToString()!,
                    ContactNumber = reader["ContactNumber"]?.ToString()!,
                    VisitDate = reader["VisitDate"]?.ToString()!,
                    VisitorPass = reader["VisitorPass"]?.ToString()!,
                    InTime = reader["InTime"]?.ToString()!,
                    ExitTime = reader["ExitTime"]?.ToString()!,
                    Address = reader["Address"]?.ToString()!,
                    WhomToMeet = reader["WhomToMeet"]?.ToString()!,
                    ReasonToMeet = reader["ReasonToMeet"]?.ToString()!,
                    Department = reader["Department"]?.ToString()!
                });
            }

            await reader.CloseAsync();
            await context.Database.CloseConnectionAsync();
            return result;

        }
        public async Task<bool> DeleteVisitorByIdRepo(VisitorById visitorById)
        {
            var parameters = new[]
            {
                new OracleParameter("p_VisitorId",visitorById.VisitorId),
                new OracleParameter("p_Result", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                }
            };

            await context.Database.ExecuteSqlRawAsync(
                @"BEGIN Sp_DeleteVisitor(:p_VisitorId, :p_Result);END;",
                parameters);

            var result = (Oracle.ManagedDataAccess.Types.OracleDecimal)parameters[1].Value;
            return result.ToInt32() == 1;
        }
        public async Task<IEnumerable<IdProofMaster>> GetIdProofListRepo()
        {
            var Ids =  await context.IdProofMasters
                .Where(d => d.IsActive == '1' && d.IsDelete == '0')
                .ToListAsync();
            return Ids;
        }
        public async Task<IEnumerable<DepartmentMaster>> GetDepartmentListRepo()
        {
            var departments = await context.DepartmentMasters
                                    .Where(d => d.IsActive == '1' && d.IsDelete == '0')
                                    .ToListAsync();

            return departments!;
        }
        public Task<IActionResult> GetVisitorCountRepo(VisitorCountPayload visitorCountPayload)
        {
            throw new NotImplementedException();
        }
    }   
}
    
   