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
        public Task<string> GetContainer(string containerName);
    }

    public class Containers : IContainers
    {
        private readonly BlobServiceClient _blobService;
        public Containers(IOptions<AzureBlobSettings> options, IConfiguration configuration)
        {
            var option = options.Value;
            _blobService = new BlobServiceClient(option.ConnectionString);
        }

        public async Task<List<string>> GetAllContainers()
        {
            var containers = new List<string>();

            await foreach(BlobContainerItem items in _blobService.GetBlobContainersAsync())
            {
                containers.Add(items.Name);
            }

            return containers;
        }

        public async Task<string> GetContainer(string containerName)
        {
            var containerClient = _blobService.GetBlobContainerClient(containerName);

            var container = await containerClient.GetAccountInfoAsync();
            var tt = containerClient.Name;

            return tt;

        }
    }

    public class Container
    {
        [Required]
        public string ContainerName { get; set; } = string.Empty;
    }

}
