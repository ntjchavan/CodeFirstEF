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

        [HttpPost("createcontainer")]
        public async Task<IActionResult> CreateContainer(string containerName)
        {
            if(string.IsNullOrEmpty(containerName))
            {
                return BadRequest("Container name should not be blank");
            }

            var response = await _containers.CreateContainer(containerName);
            return Ok(response);
        }

        [HttpDelete("deletecontainer")]
        public async Task<IActionResult> DeleteContainer(string containerName)
        {
            if(string.IsNullOrEmpty (containerName))
            {
                return BadRequest("Container name should not be empty");
            }

            var response = await _containers.DeleteContainer(containerName);
            return Ok(response);
        }

    }
}
