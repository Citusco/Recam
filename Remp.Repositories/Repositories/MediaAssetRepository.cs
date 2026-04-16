using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<MediaAsset>> GetAssetsAsync(int listingCaseId)
    {
        List<MediaAsset> mediaAssets = await _dbcontext.MediaAssets.Where(p => p.ListingCaseId == listingCaseId && !p.IsDeleted).ToListAsync();
        return mediaAssets;
    }
}
