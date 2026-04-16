using Remp.Models.Entities;

namespace Remp.Repositories.Interfaces;

public interface IMediaAssetRepository
{
    Task<MediaAsset> CreateAsync(MediaAsset mediaAsset);
}
