namespace MFL_VisitorManagement.Service;

public class ManageVisitorService(ILogger logger, 
                                  IManageVisitorRepository manageVisitorRepository,
                                  Utilities utilities,
                                  IEmailService emailService) : IManageVisitorService
{
    public async Task<IActionResult> AddVisitor(AddVisitorPaylaod addVisitorPaylaod)
    {
        logger.Information("ManageVisitorService/AddVisitor");
        MessageData data = new MessageData();
        try
        {
            var result = await manageVisitorRepository.AddVisitorRepo(addVisitorPaylaod);
            var qrCodeData = addVisitorPaylaod.Adapt<QrCodeData>();
            using var qrStream = Utilities.GenerateQRCode(qrCodeData);
            string contentId = "qrcode123";

            if (!result.IsNullOrEmpty())
            {
                var EmailRequest = new EmailRequest
                {
                    ToEmail = addVisitorPaylaod.Email,
                    VisitorPass = result,
                    Subject = "Visiting Invitation from AWC Software Noida",
                    QrStream = qrStream,
                    ContentId = contentId,
                    Body = $@"
                                <!DOCTYPE html>
                                <html>
                                <head>
                                    <meta charset=""utf-8"">
                                    <title>Visiting Request</title>
                                    <style>
                                        body {{
                                            font-family: Arial, sans-serif;
                                            background-color: #f4f4f4;
                                            padding: 20px;
                                        }}
                                        .email-container {{
                                            background-color: #ffffff;
                                            padding: 20px;
                                            border-radius: 8px;
                                            box-shadow: 0 0 10px rgba(0,0,0,0.1);
                                            max-width: 600px;
                                            margin: auto;
                                        }}
                                        .title {{
                                            font-size: 20px;
                                            color: #333333;
                                            margin-bottom: 15px;
                                        }}
                                        .content {{
                                            font-size: 16px;
                                            color: #555555;
                                        }}
                                        .pass {{
                                            font-size: 18px;
                                            font-weight: bold;
                                            color: #007BFF;
                                        }}
                                    </style>
                                </head>
                                <body>
                                    <div class='email-container'>
                                        <div class='title'>Visiting Invitation</div>
                                        <div class='content'>
                                            Hi {addVisitorPaylaod.FirstName} {addVisitorPaylaod.LastName},<br/><br/>
                                            You are invited to visit <strong>AWC Software</strong>.<br/>
                                            Your visitor pass ID is: <span class='pass'>{result}</span><br/><br/>
                                            Please bring a valid ID proof and arrive on time.<br/><br/>
                                            <strong>Scan this QR Code at entry:</strong><br/><br/>
                                            <img src='cid:{contentId}' style='width:200px; height=200px' /><br/><br/>
                                            Regards,<br/>
                                            AWC Software Admin Team
                                        </div>
                                    </div>
                                </body>
                                </html>"

                };

                var response = await emailService.SendEmailWithQrCodeAsync(EmailRequest);
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
    public async Task<IActionResult> DeleteVisitorById(VisitorById visitorById)
    {
        logger.Information("ManageVisitorService/DeleteVisitorById");
        MessageData data = new MessageData();
        try
        {
            var result = await manageVisitorRepository.DeleteVisitorByIdRepo(visitorById);
            if (result)
            {
                data.Message = "Visitor deleted successfully";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 1;
            }
            else
            {
                data.Message = "Failed to delete visitor";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Fatal("Exception from ManageVisitorService/DeleteVisitorById");
            return await utilities.GetException(ex.Message, "201");
        }

        return new JsonResult(new
        {
            data.Message,
            data.StatusCode,
            data.Token
        });
    }
    public async Task<IActionResult> GetIdProofList()
    {
        logger.Information("ManageVisitorService/GetIdProofList");
        MessageData data = new MessageData();
        try
        {
            var result = await manageVisitorRepository.GetIdProofListRepo();
            if (result.Any())
            {
                data.Data = result;
                data.Message = "Id Proof document list fethced successfully";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 1;
            }
            else
            {
                data.Data = null!;
                data.Message = "Failed to fetch Id Proof document list";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Fatal("Exception from ManageVisitorService/GetIdProofList");
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
    public async Task<IActionResult> GetDepartmentList()
    {
        logger.Information("ManageVisitorService/GetDepartmentList");
        MessageData data = new MessageData();
        try
        {
            var result = await manageVisitorRepository.GetDepartmentListRepo();
            if (result.Any())
            {
                data.Data = result;
                data.Message = "Department list fethced successfully";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 1;
            }
            else
            {
                data.Data = null!;
                data.Message = "Failed to fetch department list";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Fatal("Exception from ManageVisitorService/GetDepartmentList");
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
    public async Task<IActionResult> GetVisitorCount()
    {
        logger.Information("ManageVisitorService/GetVisitorCount");
        MessageData data = new MessageData();
        try
        {
            var result = await manageVisitorRepository.GetVisitorCountRepo();
            if (result != null)
            {
                data.Data = result.FirstOrDefault()!;
                data.Message = "Visitor count fetched successfully";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 1;
            }
            else
            {
                data.Data = null!;
                data.Message = "No visitors available";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Fatal("Exception from ManageVisitorService/GetVisitorCount");
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

    public async Task<IActionResult> GetMenuItems(RoleIdPayload roleIdPayload)
    {
        logger.Information("ManageVisitorService/GetMenuItems");
        MessageData data = new MessageData();
        try
        {
            var result = await manageVisitorRepository.GetMenuItemsRepo(roleIdPayload);
            if (result != null)
            {
                data.Data = result;
                data.Message = "Menu Items fetched successfully";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 1;
            }
            else
            {
                data.Data = null!;
                data.Message = "Failed to fetched menu items";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Fatal("Exception from ManageVisitorService/GetMenuItems");
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

    public async Task<IActionResult> UpdateVisitorRequestStatus(UpdateVisitorRequestPayload updateVisitorRequestPayload)
    {
        logger.Information("ManageVisitorService/UpdateVisitorRequestStatus");
        MessageData data = new MessageData();
        try
        {
            var result = await manageVisitorRepository.UpdateVisitorRequestStatusRepo(updateVisitorRequestPayload);
            if (result > 0)
            {
                var EmailRequest = new EmailRequest
                {
                    ToEmail = updateVisitorRequestPayload.Email,
                    VisitorPass = updateVisitorRequestPayload.VisitorPass,
                    Subject = $"Visiting Request is {(updateVisitorRequestPayload.Status == "Approved" ? "Approved" : "Rejected")}",
                    Body = $@"
                                <html>
                                  <head>
                                    <style>
                                      body {{
                                        background-color: #f9f9f9;
                                        font-family: Arial, sans-serif;
                                        color: #333333;
                                        line-height: 1.5;
                                      }}
                                      .container {{
                                        background-color: #ffffff;
                                        border: 1px solid #dddddd;
                                        border-radius: 8px;
                                        max-width: 600px;
                                        margin: 20px auto;
                                        padding: 20px;
                                      }}
                                      h2 {{
                                        color: #2a9df4;
                                        margin-top: 0;
                                      }}
                                      p {{
                                        font-size: 14px;
                                        margin: 0 0 15px 0;
                                      }}
                                      .pass-code {{
                                        font-size: 18px;
                                        color: #555555;
                                        background-color: #f1f1f1;
                                        padding: 10px;
                                        border-radius: 4px;
                                        display: inline-block;
                                      }}
                                    </style>
                                  </head>
                                  <body>
                                    <div class='container'>
                                      <h2>Visitor Access Update</h2>
                                      <p>Dear <b>{updateVisitorRequestPayload.FirstName} {updateVisitorRequestPayload.LastName}</b>,</p>
                                      <p>Your visiting request has been 
                                        <b style='color:{(updateVisitorRequestPayload.Status == "Approved" ? "green" : "red")}'>{updateVisitorRequestPayload.Status}</b>.
                                        {(updateVisitorRequestPayload.Status == "Approved"
                                      ? $"Please use the visitor pass code below upon arrival."
                                      : "Unfortunately, your access has been denied.")}
                                      </p>
                                      {(updateVisitorRequestPayload.Status == "Approved"
                                    ? $"<p class='pass-code'><b>{updateVisitorRequestPayload.VisitorPass}</b></p>"
                                    : "")}
                                      <p>Thank you.<br/>AWC Software Security Team</p>
                                    </div>
                                  </body>
                                </html>"
                };

                var response = await emailService.SendEmailAsync(EmailRequest);
                data.Message = "Status updated successfully";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 1;
            }
            else
            {
                data.Data = null!;
                data.Message = "Failed to update status";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Fatal("Exception from ManageVisitorService/UpdateVisitorRequestStatus");
            return await utilities.GetException(ex.Message, "201");
        }

        return new JsonResult(new
        {
            data.Message,
            data.StatusCode,
            data.Token
        });
    }

    public async Task<IActionResult> CheckIfVisitorExists(VisitorPass_EmailPayload visitorPass_EmailPayload)
    {
        logger.Information("ManageVisitorService/CheckIfVisitorExists");
        MessageData data = new MessageData();
        try
        {
            var (result, VisitingOfficialEmail, firstName, lastName, VisitingOfficialName, VisitorId) = await manageVisitorRepository.CheckIfVisitorExistsRepo(visitorPass_EmailPayload);
            if (result > 0)
            {
                string approved = "Approved";
                string rejected = "Rejected";
                var EmailRequest = new EmailRequest
                {
                    Subject = $"Visitor arrived with visitorPass {visitorPass_EmailPayload.VisitorPass}",
                    ToEmail = VisitingOfficialEmail,
                    FirstName = firstName,
                    LastName = lastName,
                    Body = $@"
                                <html>
                                <head>
                                    <style>
                                    body {{
                                        font-family: Arial, sans-serif;
                                        background-color: #f4f4f4;
                                        margin: 0;
                                        padding: 0;
                                    }}
                                    .container {{
                                        background-color: #ffffff;
                                        margin: 40px auto;
                                        padding: 30px;
                                        max-width: 600px;
                                        border-radius: 8px;
                                        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                                    }}
                                    .header {{
                                        font-size: 22px;
                                        font-weight: bold;
                                        margin-bottom: 20px;
                                        color: #333333;
                                    }}
                                    .content {{
                                        font-size: 16px;
                                        color: #555555;
                                        line-height: 1.6;
                                    }}
                                    .btn {{
                                        display: inline-block;
                                        margin: 20px 10px 0 0;
                                        padding: 12px 24px;
                                        border-radius: 6px;
                                        text-decoration: none;
                                        color: white;
                                        font-weight: bold;
                                    }}
                                    .approve {{
                                        background-color: #28a745;
                                    }}
                                    .reject {{
                                        background-color: #dc3545;
                                    }}
                                    .footer {{
                                        margin-top: 30px;
                                        font-size: 12px;
                                        color: #aaaaaa;
                                    }}
                                    </style>
                                </head>
                                <body>
                                    <div class='container'>
                                    <div class='header'>Visitor Arrival Notification</div>
                                    <div class='content'>
                                        Dear {VisitingOfficialName},<br><br>
                                        A visitor, {firstName} {lastName}, has arrived at the entry gate and is waiting for your approval to proceed with the meeting.<br><br>
                                        <strong>Visitor Pass:</strong> {visitorPass_EmailPayload.VisitorPass}<br>
                                        Please click one of the buttons below to approve or reject the meeting request:
                                    </div>

                                    <a href='http://192.168.0.169:7045/api/NotifyVisitor/UpdateVisitorRequestStatus?visitorPass={visitorPass_EmailPayload.VisitorPass}&email={visitorPass_EmailPayload.VisitorEmail}&firstName={firstName}&lastName={lastName}&status={approved}&visitorId={VisitorId}' class='btn approve'>Approve</a>
                                    <a href='http://192.168.0.169:7045/api/NotifyVisitor/UpdateVisitorRequestStatus?visitorPass={visitorPass_EmailPayload.VisitorPass}&email={visitorPass_EmailPayload.VisitorEmail}&firstName={firstName}&lastName={lastName}&status={rejected}&visitorId={VisitorId}' class='btn reject'>Reject</a>

                                    <div class='footer'>
                                        This is an automated message. Please do not reply directly to this email.
                                    </div>
                                    </div>
                                </body>
                                </html>"

                };
                await emailService.SendEmailAsync(EmailRequest);  
                data.Message = "Visitor Exists Already";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 1;
            }
            else
            {
                data.Message = "Visitor does not exists";
                data.StatusCode = StatusCodes.Status200OK.ToString();
                data.Token = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Fatal("Exception from ManageVisitorService/CheckIfVisitorExists");
            return await utilities.GetException(ex.Message, "201");
        }

        return new JsonResult(new
        {
            data.Message,
            data.StatusCode,
            data.Token
        });
    }



    //public static void SendEmailWithEmbeddedQRCode(string toEmail, string subject, QrCodeData qrData)
    //{
    //    var fromAddress = new MailAddress("squreshi0507@gmail.com", "AWC Software");
    //    var toAddress = new MailAddress(toEmail);
    //    const string fromPassword = "lnjz cjmf pqck kogq";

    //    var smtp = new SmtpClient
    //    {
    //        Host = "smtp.gmail.com", // e.g., smtp.gmail.com
    //        Port = 587,
    //        EnableSsl = true,
    //        DeliveryMethod = SmtpDeliveryMethod.Network,
    //        UseDefaultCredentials = false,
    //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
    //    };

    //    using var qrStream = Utilities.GenerateQRCode(qrData);

    //    var message = new MailMessage(fromAddress, toAddress)
    //    {
    //        Subject = subject,
    //        IsBodyHtml = true
    //    };

    //    // Set CID (Content-ID) for the image
    //    string contentId = "qrcode123";
    //    string htmlBody = $@"
    //                        <html>
    //                            <body>
    //                                <p>Here is your QR Code:</p>
    //                                <img src='cid:{contentId}' />
    //                            </body>
    //                        </html>";

    //    // Create alternate view with linked resource
    //    var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

    //    var qrImage = new LinkedResource(qrStream, MediaTypeNames.Image.Jpeg)
    //    {
    //        ContentId = contentId,
    //        TransferEncoding = TransferEncoding.Base64,
    //        ContentType = new ContentType("image/png")
    //    };

    //    htmlView.LinkedResources.Add(qrImage);
    //    message.AlternateViews.Add(htmlView);

    //    smtp.Send(message);
    //}
}
