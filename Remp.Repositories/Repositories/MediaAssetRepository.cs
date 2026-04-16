using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;

namespace Remp.Repositories.Repositories;

public class MediaAssetRepository : IMediaAssetRepository
{
    private readonly RempDbContext _dbcontext;

    public MediaAssetRepository(RempDbContext dbContext)
    {
        _dbcontext = dbContext;
    }

    public async Task<MediaAsset> CreateAsync(MediaAsset mediaAsset)
    {
        _dbcontext.MediaAssets.Add(mediaAsset);
        int changes = await _dbcontext.SaveChangesAsync();

        if (changes == 0)
        {
            throw new InvalidOperationException("Create media asset failed.");
        }
        
        return mediaAsset;
    }
}
