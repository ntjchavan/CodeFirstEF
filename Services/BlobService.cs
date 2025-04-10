using Azure.Storage.Blobs;
using CodeFirstEFAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CodeFirstEFAPI.Services
{
    public class BlobService: IBlobService
    {
        private readonly BlobContainerClient _blobContainerClient;
        public BlobService(IOptions<AzureBlobSettings> options)
        {
            var settings = options.Value;
            _blobContainerClient = new BlobContainerClient(settings.ConnectionString, settings.ContainerName);
            _blobContainerClient.CreateIfNotExists(); //optional
        }

        public async Task<string> FileUploadAsync(IFormFile file)
        {
            var blobClient = _blobContainerClient.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }
    }
}
