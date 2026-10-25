using Azure.Storage.Blobs;
using CodeFirstEFAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CodeFirstEFAPI.Services
{
    public interface IBlobService
    {
        public Task<string> FileUploadByContainerNameAsync(string containerName, IFormFile file);

        public Task<bool> FileDeleteByContainerNameAndFileName(string containerName, string fileName);

        public Task<IDictionary<string, string>> GetBlobMetadata(string containerName, string blobName);

        public Task<List<string>> GetAllBlobsByContainerName(string containerName);

        public Task<(Stream content, string contentType)> BlobDownload(string containerName, string blobName);
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

        public async Task<IDictionary<string, string>> GetBlobMetadata(string containerName, string blobName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                return new Dictionary<string, string>();

            var properties = await blobClient.GetPropertiesAsync();

            var metadata = properties.Value.Metadata;

            return metadata;
        }

        public async Task<List<string>> GetAllBlobsByContainerName(string containerName)
        {
            var blobLists = new List<string>();

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobLists.Add(blobItem.Name);
            }

            return blobLists;
        }

        public async Task<(Stream content, string contentType)> BlobDownload(string containerName, string blobName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                return (null, null);

            var downloadInfo = await blobClient.DownloadStreamingAsync();

            return (downloadInfo.Value.Content, downloadInfo.Value.Details.ContentType);

        }
    }

}
