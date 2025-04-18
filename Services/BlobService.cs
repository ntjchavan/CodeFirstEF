using Azure.Storage.Blobs;
using CodeFirstEFAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CodeFirstEFAPI.Services
{
    public interface IBlobService
    {
        public Task<string> FileUploadAsync(IFormFile file);
        public Task<string> FileUploadByContainerNameAsync(string containerName, IFormFile file);

        public Task<bool> FileDeleteByContainerNameAndFileName(string containerName, string fileName);
    }

    public class BlobService: IBlobService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(IOptions<AzureBlobSettings> options, BlobServiceClient blobServiceClient)
        {
            var settings = options.Value;
            _blobContainerClient = new BlobContainerClient(settings.ConnectionString, settings.ContainerName);
            _blobContainerClient.CreateIfNotExists(); //optional

            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> FileUploadAsync(IFormFile file)
        {
            var blobClient = _blobContainerClient.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<string> FileUploadByContainerNameAsync(string containerName, IFormFile fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(); // optional, will created container if not exists

            var blobClient = containerClient.GetBlobClient(fileName.FileName);

            using (var stread = fileName.OpenReadStream())
            {
                await blobClient.UploadAsync(stread, overwrite: true);
            }
            return blobClient.Uri.ToString();

        }

        public async Task<bool> FileDeleteByContainerNameAndFileName(string containerName, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobName = containerClient.GetBlobClient(fileName);

            var response = await blobName.DeleteIfExistsAsync();

            return response;
        }
    }

}
