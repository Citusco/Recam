using Microsoft.AspNetCore.Http;

namespace Remp.Service.Interfaces;

public interface IBlobUploadService
{
    Task<string> UploadAsync(IFormFile file);
    Task DeleteAsync(string blobUrl);
}
