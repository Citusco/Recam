using System;
using Microsoft.EntityFrameworkCore;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Models.Enums;
using Remp.Repositories.Repositories;

namespace Remp.Tests;

public class MediaAssetRepositoryTests : IDisposable
{
    private readonly RempDbContext _dbContext;
    private readonly MediaAssetRepository _mediaAssetRepo;

    private MediaAsset CreateSampleMediaAsset(
        int id = 1,
        int listingCaseId = 9,
        string mediaUrl = "thisisanurl",
        MediaType mediaType = MediaType.Picture,
        string userId = "user-123"
    ) =>
        new MediaAsset
        {
            Id = id,
            ListingCaseId = listingCaseId,
            MediaUrl = mediaUrl,
            MediaType = mediaType,
            UserId = userId,
        };

    public MediaAssetRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<RempDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new RempDbContext(options);
        _mediaAssetRepo = new MediaAssetRepository(_dbContext);
    }

    // ── CreateAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ShouldReturnMediaAsset_WhenSuccess()
    {
        // Arrange
        var mediaAsset = CreateSampleMediaAsset();

        // Act
        var result = await _mediaAssetRepo.CreateAsync(mediaAsset);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(9, result.ListingCaseId);
        Assert.Equal("thisisanurl", result.MediaUrl);
        Assert.Equal(MediaType.Picture, result.MediaType);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistToDatabase()
    {
        // Arrange
        var mediaAsset = CreateSampleMediaAsset();

        // Act
        await _mediaAssetRepo.CreateAsync(mediaAsset);

        // Assert
        var saved = await _dbContext.MediaAssets.FindAsync(1);
        Assert.NotNull(saved);
        Assert.Equal("thisisanurl", saved.MediaUrl);
        Assert.Equal("user-123", saved.UserId);
    }

    // ── GetAssetsAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetAssetsAsync_ShouldReturnOnlyNonDeletedAssets()
    {
        // Arrange
        var deletedAsset = CreateSampleMediaAsset(2, 9);
        deletedAsset.IsDeleted = true;
        _dbContext.MediaAssets.AddRange(
            CreateSampleMediaAsset(1, 9),
            deletedAsset,
            CreateSampleMediaAsset(3, 9)
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _mediaAssetRepo.GetAssetsAsync(9);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, m => Assert.False(m.IsDeleted));
    }

    [Fact]
    public async Task GetAssetsAsync_ShouldReturnOnlyAssetsForListingCase()
    {
        // Arrange
        _dbContext.MediaAssets.AddRange(
            CreateSampleMediaAsset(1, 9),
            CreateSampleMediaAsset(2, 99)
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _mediaAssetRepo.GetAssetsAsync(9);

        // Assert
        Assert.Single(result);
        Assert.All(result, m => Assert.Equal(9, m.ListingCaseId));
    }

    // ── GetByIdAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMediaAsset_WhenExists()
    {
        // Arrange
        _dbContext.MediaAssets.Add(CreateSampleMediaAsset());
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _mediaAssetRepo.GetByIdAsync(1, "user-123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("user-123", result.UserId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Act
        var result = await _mediaAssetRepo.GetByIdAsync(999, "user-123");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDeleted()
    {
        // Arrange
        var deletedAsset = CreateSampleMediaAsset();
        deletedAsset.IsDeleted = true;
        _dbContext.MediaAssets.Add(deletedAsset);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _mediaAssetRepo.GetByIdAsync(1, "user-123");

        // Assert
        Assert.Null(result);
    }

    // ── DeleteAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_ShouldSoftDelete_WhenExists()
    {
        // Arrange
        _dbContext.MediaAssets.Add(CreateSampleMediaAsset());
        await _dbContext.SaveChangesAsync();

        // Act
        await _mediaAssetRepo.DeleteAsync(1);

        // Assert
        var deleted = await _dbContext.MediaAssets.FindAsync(1);
        Assert.NotNull(deleted);
        Assert.True(deleted.IsDeleted);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _mediaAssetRepo.DeleteAsync(999));
    }

    // ── ExistAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task ExistAsync_ShouldReturnTrue_WhenExists()
    {
        // Arrange
        _dbContext.MediaAssets.Add(CreateSampleMediaAsset());
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _mediaAssetRepo.ExistAsync(1, "user-123");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistAsync_ShouldReturnFalse_WhenNotFound()
    {
        // Act
        var result = await _mediaAssetRepo.ExistAsync(999, "user-123");

        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
