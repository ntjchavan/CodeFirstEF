using CodeFirstEFAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstEFAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandardController : ControllerBase
    {
        private readonly StudentDBContext _studentDBContext;
        public StandardController(StudentDBContext studentDBContext) 
        {
            this._studentDBContext = studentDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetStandards()
        {
            var standardList = await this._studentDBContext.Standards.ToListAsync();

            if(standardList == null || !standardList.Any())
            {
                return NotFound("No classes founds");
            }

            return Ok(standardList);
        }

        [HttpGet("error-handle")]
        public IActionResult ErrorHandle()
        {
            throw new Exception("Custom error message generated");
        }
    }
}
