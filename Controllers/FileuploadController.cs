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
        [Route("BlobUploadByContainerName")]
        public async Task<IActionResult> BlobUploadByContainerName([FromQuery] string containerName, IFormFile formFile)
        {
            var response = await _blobService.FileUploadByContainerNameAsync(containerName, formFile);

            return Ok(response);
        }

        [HttpDelete]
        [Route("blobdelete")]
        public async Task<IActionResult> BlobDelete(string containerName, string blobName)
        {
            if (string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(blobName))
            {
                return BadRequest("Container name & blob name are require.");
            }
            var result = await _blobService.FileDeleteByContainerNameAndFileName(containerName,blobName);
            if (result)
                return Ok($"Blobl {blobName} deleted successfully from container {containerName}");

            return NotFound($"Blob {blobName} not found in container {containerName}");
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
