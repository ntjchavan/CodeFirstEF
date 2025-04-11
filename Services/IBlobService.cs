using Microsoft.AspNetCore.Http;

namespace CodeFirstEFAPI.Services
{
    public interface IBlobService
    {
        public Task<string> FileUploadAsync(IFormFile file);
    }
}
