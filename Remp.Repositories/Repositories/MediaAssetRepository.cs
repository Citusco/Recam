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

    public async Task DeleteAsync(int mediaId)
    {
        MediaAsset? mediaAsset = await _dbcontext.MediaAssets.FindAsync(mediaId);
        if (mediaAsset == null)
        {
            throw new KeyNotFoundException("Media asset not found");
        }
        mediaAsset.IsDeleted = true;
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(int mediaId, string userId)
    {
        return await _dbcontext.MediaAssets.AnyAsync(p => p.Id == mediaId && p.UserId == userId);
    }
}