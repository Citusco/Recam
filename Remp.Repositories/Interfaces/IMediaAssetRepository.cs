using Remp.Models.Entities;

namespace Remp.Repositories.Interfaces;

public interface IMediaAssetRepository
{
    Task<MediaAsset> CreateAsync(MediaAsset mediaAsset);
    Task<IEnumerable<MediaAsset>> GetAssetsAsync(int listingCaseId);
    Task<IEnumerable<MediaAsset>> GetByIdsAsync(IEnumerable<int> mediaIds);
    Task<bool> ExistAsync(int mediaId, string userId);
    Task DeleteAsync(int mediaId);
}
