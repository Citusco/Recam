using Microsoft.EntityFrameworkCore;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;

namespace Remp.Repositories.Repositories;

public class SelectedMediaAssetRepository : ISelectedMediaAssetRepository
{
    private readonly RempDbContext _dbcontext;

    public SelectedMediaAssetRepository(RempDbContext dbContext)
    {
        _dbcontext = dbContext;
    }

    public async Task<IEnumerable<SelectedMedia>> CreateAsync(IEnumerable<SelectedMedia> selectedMedias)
    {
        await _dbcontext.AddRangeAsync(selectedMedias);
        await _dbcontext.SaveChangesAsync();
        return selectedMedias;
    }

    public async Task DeleteByListingCaseAsync(int listingCaseId, string agentId)
    {
        IEnumerable<SelectedMedia> existing = await _dbcontext.SelectedMedias
        .Where(p => p.ListingCaseId == listingCaseId && p.AgentId == agentId)
        .ToListAsync();

        _dbcontext.SelectedMedias.RemoveRange(existing);
        await _dbcontext.SaveChangesAsync();
    }
}
