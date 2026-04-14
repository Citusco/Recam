using Remp.Models.Entities;

namespace Remp.Repositories.Interfaces;

public interface IListingCaseRepository
{
    Task<ListingCase> CreateAsync (ListingCase listingCase);
}