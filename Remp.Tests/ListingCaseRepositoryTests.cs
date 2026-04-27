using Microsoft.EntityFrameworkCore;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Models.Enums;
using Remp.Repositories.Repositories;

namespace Remp.Tests;

public class ListingCaseRepositoryTests : IDisposable
{
    private readonly RempDbContext _dbContext;
    private readonly ListingCaseRepository _repository;

    public ListingCaseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<RempDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new RempDbContext(options);
        _repository = new ListingCaseRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    private ListingCase CreateSampleListingCase(int id = 1, string userId = "user-123") =>
        new ListingCase
        {
            Id = id,
            UserId = userId,
            Title = "Test Property",
            Description = "Test Description",
            Street = "123 Test St",
            City = "Test City",
            State = "Test State",
            ListCaseStatus = ListCaseStatus.Created,
            IsDeleted = false,
            CreatedAt = DateTime.Now,
        };

    // ── CreateAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ShouldReturnListingCase_WhenSuccess()
    {
        // Arrange
        var listingCase = CreateSampleListingCase();

        // Act
        var result = await _repository.CreateAsync(listingCase);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Property", result.Title);
        Assert.Equal("user-123", result.UserId);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistToDatabase()
    {
        // Arrange
        var listingCase = CreateSampleListingCase();

        // Act
        await _repository.CreateAsync(listingCase);

        // Assert
        var saved = await _dbContext.ListingCases.FindAsync(1);
        Assert.NotNull(saved);
        Assert.Equal("Test Property", saved.Title);
    }

    // ── GetAllAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyAdminCases_WhenRoleIsAdmin()
    {
        // Arrange
        var adminId = "admin-123";
        var otherAdminId = "admin-456";

        _dbContext.ListingCases.AddRange(
            CreateSampleListingCase(1, adminId),
            CreateSampleListingCase(2, otherAdminId)
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync(adminId, "Admin");

        // Assert
        Assert.Single(result);
        Assert.All(result, lc => Assert.Equal(adminId, lc.UserId));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAdminCases_WhenAdminHasMultiple()
    {
        // Arrange
        var adminId = "admin-123";

        _dbContext.ListingCases.AddRange(
            CreateSampleListingCase(1, adminId),
            CreateSampleListingCase(2, adminId),
            CreateSampleListingCase(3, adminId)
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync(adminId, "Admin");

        // Assert
        Assert.Equal(3, result.Count());
        Assert.All(result, lc => Assert.Equal(adminId, lc.UserId));
    }

    // ── DeleteAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_ShouldSoftDelete_WhenListingCaseExists()
    {
        // Arrange
        var listingCase = CreateSampleListingCase();
        _dbContext.ListingCases.Add(listingCase);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(1, "user-123");

        // Assert
        var deleted = await _dbContext.ListingCases.FindAsync(1);
        Assert.NotNull(deleted);
        Assert.True(deleted.IsDeleted);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repository.DeleteAsync(999, "user-123")
        );
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowInvalidOperationException_WhenAlreadyDeleted()
    {
        // Arrange
        var listingCase = CreateSampleListingCase();
        listingCase.IsDeleted = true;
        _dbContext.ListingCases.Add(listingCase);
        await _dbContext.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repository.DeleteAsync(1, "user-123")
        );
    }

    // ── UpdateStatus ────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateStatus_ShouldIncrementStatus()
    {
        // Arrange
        var listingCase = CreateSampleListingCase();
        listingCase.ListCaseStatus = ListCaseStatus.Created;
        _dbContext.ListingCases.Add(listingCase);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.UpdateStatus(listingCase);

        // Assert
        var updated = await _dbContext.ListingCases.FindAsync(1);
        Assert.Equal(ListCaseStatus.Pending, updated!.ListCaseStatus);
    }

    // ── ExistsAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenListingCaseExists()
    {
        // Arrange
        _dbContext.ListingCases.Add(CreateSampleListingCase());
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(1, "user-123");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenNotFound()
    {
        // Act
        var result = await _repository.ExistsAsync(999, "user-123");

        // Assert
        Assert.False(result);
    }
}
