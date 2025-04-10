using CodeFirstEFAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirstEFAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileuploadController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly IContainers _containers;

        public FileuploadController(IBlobService blobService, IContainers containers)
        {
            _blobService = blobService;
            _containers = containers;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected");
            }
            var result = await _blobService.FileUploadAsync(file);
            return Ok(result);
        }

        [HttpGet("getallcontainers")]
        public async Task<IActionResult> GetAllContainers()
        {
            var response = await _containers.GetAllContainers();
            return Ok(response);
        }

        [HttpGet]
        [Route("gelcontainer")]
        public async Task<IActionResult> GetContainer(string contaienerName)
        {
            var response = await _containers.GetContainer(contaienerName);

            return Ok(response);
        }
    }
}
