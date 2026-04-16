using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Models.Enums;
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

    public async Task<ListingCase> UpdateAsync(int listingCaseId, string userId, ListingCase updatedData)
    {
        ListingCase? existingCase = await _dbContext.ListingCases
            .FirstOrDefaultAsync(p => p.Id == listingCaseId && p.UserId == userId);

        if (existingCase == null)
            throw new KeyNotFoundException("Listing case not found.");

        existingCase.Title = updatedData.Title;
        existingCase.Description = updatedData.Description;
        existingCase.Street = updatedData.Street;
        existingCase.City = updatedData.City;
        existingCase.State = updatedData.State;
        existingCase.Postcode = updatedData.Postcode;
        existingCase.Longitude = updatedData.Longitude;
        existingCase.Latitude = updatedData.Latitude;
        existingCase.Price = updatedData.Price;
        existingCase.Bedrooms = updatedData.Bedrooms;
        existingCase.Bathrooms = updatedData.Bathrooms;
        existingCase.Garages = updatedData.Garages;
        existingCase.FloorArea = updatedData.FloorArea;
        existingCase.PropertyType = updatedData.PropertyType;
        existingCase.SaleCategory = updatedData.SaleCategory;

        await _dbContext.SaveChangesAsync();
        return existingCase;
    }

    public async Task DeleteAsync(int listingCaseId, string userId)
    {
        ListingCase? listingCase = await _dbContext.ListingCases.FirstOrDefaultAsync(p => p.Id == listingCaseId && p.UserId == userId);
        if (listingCase == null)
            throw new KeyNotFoundException("Listing case not found");
        if (listingCase.IsDeleted)
            throw new InvalidOperationException("Cannot delete deleted case.");
        listingCase.IsDeleted = true;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateStatus(ListingCase listingCase)
    {
        listingCase.ListCaseStatus += 1;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int listingCaseId, string userId)
    {
        return await _dbContext.ListingCases.AnyAsync(p => p.UserId == userId && p.Id == listingCaseId);
    }
}
