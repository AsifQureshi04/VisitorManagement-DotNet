using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MFL_VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController(ILogger _logger) : ControllerBase
    {
    }
}
