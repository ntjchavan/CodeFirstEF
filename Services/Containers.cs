using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CodeFirstEFAPI.Models;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEFAPI.Services
{
    public interface IContainers
    {
        public Task<List<string>> GetAllContainers();

        public Task<BlobContainerInfo> CreateContainer(string containerName);

        public Task<bool> DeleteContainer(string containerName);
    }

    public class Containers : IContainers
    {
        private readonly BlobServiceClient _blobServiceClient;
        public Containers(IOptions<AzureBlobSettings> _options, IConfiguration config, BlobServiceClient blobServiceClient)
        {
            var options = _options.Value;
            
            _blobServiceClient = blobServiceClient;//new BlobServiceClient(options.ConnectionString); // config["AzStorageConnString"]
            // Update to config in blobServiceClient program.cs file builder.Services.AddSingleton(config => new BlobServiceClient(builder.Configuration.GetValue<string>("AzStorageConnString")));
        }

        public async Task<List<string>> GetAllContainers()
        {
            var containers = new List<string>();

            await foreach(BlobContainerItem items in _blobServiceClient.GetBlobContainersAsync())
            {
                containers.Add(items.Name);
            }

            return containers;
        }

        public async Task<BlobContainerInfo> CreateContainer(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var result = await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            //result.GetRawResponse().Status; // Get 201 response
            return result;

        }

        public async Task<bool> DeleteContainer(string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var result = await containerClient.DeleteIfExistsAsync();

            return result;
        }

    }

    public class Container
    {
        [Required]
        public string ContainerName { get; set; } = string.Empty;
    }

}
