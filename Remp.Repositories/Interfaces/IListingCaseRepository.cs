using Remp.Models.Entities;

namespace Remp.Repositories.Interfaces;

public interface IListingCaseRepository
{
    Task<ListingCase> CreateAsync (ListingCase listingCase);
    Task<IEnumerable<ListingCase>> GetAllAsync(string userId, string role);
    Task<ListingCase?> GetAsync(int listingcaseId, string userId, string role);
    Task<ListingCase> UpdateAsync(int listingCaseId, string userId, ListingCase updatedData);
    Task DeleteAsync(int listingCaseId, string userId);
}