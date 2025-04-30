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
        [Route("upload-blob")]
        public async Task<IActionResult> BlobUploadByContainerName([FromQuery] string containerName, IFormFile formFile)
        {
            var response = await _blobService.FileUploadByContainerNameAsync(containerName, formFile);

            return Ok(response);
        }

        [HttpGet("list-blobs")]
        public async Task<IActionResult> GetAllListBlobs([FromQuery] string containerName)
        {
            var response = await _blobService.GetAllBlobsByContainerName(containerName);

            if(!response.Any())
            {
                return NotFound("No blobs found in container - " + containerName);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("get-blobs-metadata")]
        public async Task<IActionResult> GetBlobMetaData(string containerName, string blobName)
        {
            var response = await _blobService.GetBlobMetadata(containerName, blobName);

            if (response.Count == 0)
            {
                return NotFound($"Blob not found in container: {containerName}, with name: {blobName}");
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("blob-delete")]
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

        [HttpGet("blob-download")]
        public async Task<IActionResult> BlobDownload([FromQuery] string containerName, [FromQuery] string blobName)
        {
            var (content, contentType) = await _blobService.BlobDownload(containerName, blobName);

            if (content == null)
                return NotFound($"Blob not found - {blobName}");

            return File(content, contentType ?? "application/octet-stream", blobName);
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
