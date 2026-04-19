using Remp.Models.Entities;

namespace Remp.Repositories.Interfaces;

public interface ISelectedMediaAssetRepository
{
    Task DeleteByListingCaseAsync(int listingCaseId, string agentId);
    Task<IEnumerable<SelectedMedia>> CreateAsync(IEnumerable<SelectedMedia> selectedMedias);
}