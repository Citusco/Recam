using Microsoft.AspNetCore.Http;
using Remp.Models.Enums;
using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IMediaAssetService
{
    Task<IEnumerable<CreateMediaAssetResponseDto>> CreateAsync (
        List<IFormFile> files,
        MediaType mediaType,
        int listingCaseId,
        string userId  
    );

    Task<IEnumerable<MediaAssetResponseDto>> GetAsync(int listingCaseId, string userId, string role);
}
