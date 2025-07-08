

namespace MFL_VisitorManagement.Repositories;

public class ManageVisitorRepository(DataContext context) : IManageVisitorRepository
{
    public async Task<(string VisitorPass,string VisitingOfficialEmail, int VisitorId)> AddVisitorRepo(AddVisitorPaylaod addVisitorPaylaod)
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
            },
            new OracleParameter("p_VisitorPass", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Output,
                Size = 20
            },
            new OracleParameter("p_VisitingOfficialEmail", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Output,
                Size = 100
            },
            new OracleParameter("p_Visitorid", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            }
        };

        await context.Database.ExecuteSqlRawAsync(
            @"BEGIN Sp_AddVisitor(
            :p_FirstName, :p_LastName, :p_Email, :p_PhoneNumber,
            :p_Address, :p_WhomToMeet, :p_Department,
            :p_IdProof, :p_IdProofNumber, :p_ReasonToMeet,
            :p_VisitDate, :p_InTime,:p_VisitorPass,:p_VisitingOfficialEmail,:p_Visitorid); END;", parameters);

        string VisitorPass = parameters[12].Value.ToString()!;
        string VisitingOfficialEmail = parameters[13].Value.ToString()!;
        int VisitorId = Convert.ToInt32(((OracleDecimal)parameters[14].Value).Value);

        return (VisitorPass, VisitingOfficialEmail, VisitorId);
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
                Department = reader["Department"]?.ToString()!,
                Status  = reader["Status"]?.ToString()!
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
        return result.ToInt32() > 1;
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
    public async Task<IEnumerable<VisitorCountDto>> GetVisitorCountRepo()
    {
        var parameters =

            new OracleParameter("o_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };
        

        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "Sp_VisitorCount";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(parameters);

        await context.Database.OpenConnectionAsync();
        var reader = await command.ExecuteReaderAsync();

        var result = new List<VisitorCountDto>();

        while(await reader.ReadAsync())
        {
            result.Add(new VisitorCountDto
            {
                TodayVisitors = Convert.ToInt32(reader["TodaysVisitor"]),
                YesterdayVisitors = Convert.ToInt32(reader["YesterdaysVisitor"]),
                LastWeekVisitors = Convert.ToInt32(reader["LastWeekVisitor"]),
                TotalVisitors = Convert.ToInt32(reader["TotalVisitor"])
            });
        }

        await reader.CloseAsync();
        await context.Database.CloseConnectionAsync();
        return result;
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsRepo(RoleIdPayload roleIdPayload)
    {
        var parameters = new[]
        {
            new OracleParameter("p_Role", roleIdPayload.RoleName),
            new OracleParameter("o_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            }
        };

        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "Sp_GetMenusWithSubMenus";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddRange(parameters);

        await context.Database.OpenConnectionAsync();
        var reader = await command.ExecuteReaderAsync();

        var menuDictionary = new Dictionary<int, MenuItem>();

        while (await reader.ReadAsync())
        {
            var menuId = Convert.ToInt32(reader["MenuId"]);

            if (!menuDictionary.TryGetValue(menuId, out var menuItem))
            {
                menuItem = new MenuItem
                {
                    MenuId = menuId,
                    MenuName = reader["MenuName"].ToString()!,
                    MenuIcon = reader["MenuIcon"].ToString()!,
                    MenuRoute = reader["MenuRoute"].ToString()!,
                    SubMenuItem = new List<SubMenuItem>()
                };

                menuDictionary.Add(menuId, menuItem);
            }

            if (reader["SubMenuId"] != DBNull.Value)
            {
                var subMenu = new SubMenuItem
                {
                    SubMenuId = Convert.ToInt32(reader["SubMenuId"]),
                    SubMenuName = reader["SubMenuName"].ToString()!,
                    SubMenuIcon = reader["SubMenuIcon"].ToString()!,
                    SubMenuRoute = reader["SubMenuRoute"].ToString()!
                };

                menuItem.SubMenuItem.Add(subMenu);
            }
        }

        await reader.CloseAsync();
        await context.Database.CloseConnectionAsync();

        return menuDictionary.Values;
    }

    public async Task<int> UpdateVisitorRequestStatusRepo(UpdateVisitorRequestPayload updateVisitorRequestPayload)
    {
        var parameters = new[]
        {
            new OracleParameter("p_Status",updateVisitorRequestPayload.Status),
            new OracleParameter("p_VisitorId",updateVisitorRequestPayload.VisitorId),
            new OracleParameter("p_VisitorPass",updateVisitorRequestPayload.VisitorPass),
            new OracleParameter("p_Result", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            }
        };

        await context.Database.ExecuteSqlRawAsync(
            @"BEGIN Sp_UpdateVisitorRequestStatus(:p_Status,:p_VisitorId,:p_VisitorPass,:p_Result);END;", parameters);

        return Convert.ToInt32(parameters[3].Value.ToString());
   
    }

    public async Task<(int Result, string VisitingOfficialEmail, string FirstName, string LastName, string VisitingOfficialName, int VisitorId)> CheckIfVisitorExistsRepo(VisitorPass_EmailPayload visitorPass_EmailPayload)
    {
        var parameters = new[]
        {
            new OracleParameter("p_Email", visitorPass_EmailPayload.VisitorEmail),
            new OracleParameter("p_VisitorPass", visitorPass_EmailPayload.VisitorPass),
            new OracleParameter("p_Result", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            },
            new OracleParameter("p_DummyEmail", OracleDbType.Varchar2, 200)
            {
                Direction = ParameterDirection.Output
            },
            new OracleParameter("p_FirstName", OracleDbType.Varchar2, 200)
            {
                Direction = ParameterDirection.Output
            },
            new OracleParameter("p_LastName", OracleDbType.Varchar2, 200)
            {
                Direction = ParameterDirection.Output
            },
             new OracleParameter("p_VisitingOfficialName", OracleDbType.Varchar2, 200)
            {
                Direction = ParameterDirection.Output
            },
             new OracleParameter("p_VisitorId", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            }
        };

        await context.Database.ExecuteSqlRawAsync(
            @"BEGIN Sp_CheckIfVisitorExists(:p_Email, :p_VisitorPass, :p_Result, :p_DummyEmail, :p_FirstName, :p_LastName, :p_VisitingOfficialName,:p_VisitorId); END;", parameters);

        int result = Convert.ToInt32(((OracleDecimal)parameters[2].Value).Value);
        string VisitingOfficialEmail = parameters[3].Value?.ToString()!;
        string firstName = parameters[4].Value?.ToString()!;
        string lastName = parameters[5].Value?.ToString()!;
        string VisitingOfficialName = parameters[6].Value?.ToString()!;
        int VisitorId = Convert.ToInt32(((OracleDecimal)parameters[7].Value).Value);

        return (result, VisitingOfficialEmail,firstName, lastName, VisitingOfficialName, VisitorId);
    }

    public async Task<IEnumerable<VisitorDetailsDto>> GetVisitorDetailByContactRepo(GetDetailsByMobileDto getDetailsByMobileDto)
    {
        var parameters = new[]
        {
            new OracleParameter("p_ContactNo",getDetailsByMobileDto.ContactNo),
            new OracleParameter("o_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output,
            }
        };

        return await context.VisitorDetailsDtos.FromSqlRaw(
            @"BEGIN Sp_GetVisitorDetailByContactNo(:p_ContactNo, :o_cursor);END;", parameters).ToListAsync();

    }
}

