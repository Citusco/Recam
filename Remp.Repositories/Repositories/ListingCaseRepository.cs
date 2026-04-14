using Microsoft.EntityFrameworkCore;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;

namespace Remp.Repositories.Repositories;

public class ListingCaseRepository : IListingCaseRepository
{
    private readonly RempDbContext _dbContext;

    public ListingCaseRepository(RempDbContext dbContext)
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
            throw new Exception("Failed to add listing case.");
        }
    }

    public async Task<IEnumerable<ListingCase>> GetAllAsync(string userId, string role)
    {
        IEnumerable<ListingCase> listingCases;

        // Admins get all cases created by themselves
        if (role == "Admin")
            listingCases = await _dbContext.ListingCases
                .Where(p => p.UserId == userId)
                .ToListAsync();
        else
            // Agents get all cases related to them.
            listingCases = await _dbContext.AgentListingCases
                .Where(p => p.AgentId == userId)
                .Include(a => a.ListingCase)
                .Select(a => a.ListingCase)
                .ToListAsync();

        return listingCases;
    }

    public async Task<ListingCase?> GetAsync(int listingcaseId, string userId, string role)
    {
        // Accessibility should be checked first.
        bool hasAccess = true;
        if (role != "Admin")
        {
            hasAccess = await _dbContext.AgentListingCases
                .AnyAsync(p => p.AgentId == userId && p.ListingCaseId == listingcaseId);
        }
        else
        {
            hasAccess = await _dbContext.ListingCases
            .AnyAsync(p => p.UserId == userId && p.Id == listingcaseId);
        }

        if (!hasAccess)
            throw new Exception("Access denied.");

        return await _dbContext.ListingCases.FindAsync(listingcaseId);
    }
}
