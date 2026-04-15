using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Remp.Service.Interfaces;

namespace Remp.Service.Services;

public class BlobUploadService : IBlobUploadService
{
    private readonly BlobContainerClient _containerClient;

    public BlobUploadService(IConfiguration configuration)
    {
        string connectionString = configuration["Azure:BlobConnectionString"]!;
        BlobServiceClient serviceClient = new BlobServiceClient(connectionString);
        _containerClient = serviceClient.GetBlobContainerClient("recam");
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        // Generate a unique filename to avoid collisions
        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        BlobClient blobClient = _containerClient.GetBlobClient(fileName);

        using Stream stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }
}
