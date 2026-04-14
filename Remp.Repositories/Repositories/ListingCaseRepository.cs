using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;

namespace Remp.Repositories.Repositories;

public class ListingCaseRepository : IListingCaseRepository
{
    private readonly RempDbContext _dbContext;
    
    public ListingCaseRepository (RempDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ListingCase> CreateAsync(ListingCase listingCase)
    {
        await _dbContext.ListingCases.AddAsync(listingCase);

        int changes = await _dbContext.SaveChangesAsync();

        if (changes > 0)
        {
            return listingCase;
        }
        else
        {
            throw new Exception ("Failed to add listing case.");
        }
    }
}
