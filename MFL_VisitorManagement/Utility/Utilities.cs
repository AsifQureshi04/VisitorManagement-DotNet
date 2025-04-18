using Microsoft.AspNetCore.Mvc;

namespace MFL_VisitorManagement.Utility
{
    public class Utilities
    {
        public async Task<IActionResult> GetException(string exception, string status)
        {
            return new JsonResult(new
            {
                Message = exception,
                Status = status,
                Token = 0
            }); 
        }
    }
}
