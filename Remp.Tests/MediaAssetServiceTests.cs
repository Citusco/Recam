using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using Remp.Models.Entities;
using Remp.Models.Enums;
using Remp.Repositories.Interfaces;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;
using Remp.Service.Services;

namespace Remp.Tests;

public class MediaAssetServiceTests
{
    private readonly Mock<IMediaAssetRepository> _mockMediaRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IBlobUploadService> _mockBlob;
    private readonly Mock<IListingCaseRepository> _mockListingRepo;
    private readonly Mock<ILogService> _mockLogService;
    private readonly MediaAssetService _service;

    private List<MediaAsset> CreateSampleMediaAssets() =>
        new List<MediaAsset>
        {
            new MediaAsset
            {
                Id = 1,
                MediaType = MediaType.Picture,
                MediaUrl = "url-1",
            },
            new MediaAsset
            {
                Id = 2,
                MediaType = MediaType.Picture,
                MediaUrl = "url-2",
            },
        };

    private List<MediaAssetResponseDto> CreateSampleMediaAssetDtos() =>
        new List<MediaAssetResponseDto>
        {
            new MediaAssetResponseDto
            {
                Id = 1,
                MediaType = MediaType.Picture,
                MediaUrl = "url-1",
            },
            new MediaAssetResponseDto
            {
                Id = 2,
                MediaType = MediaType.Picture,
                MediaUrl = "url-2",
            },
        };

    public MediaAssetServiceTests()
    {
        _mockMediaRepo = new Mock<IMediaAssetRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockListingRepo = new Mock<IListingCaseRepository>();
        _mockBlob = new Mock<IBlobUploadService>();
        _mockLogService = new Mock<ILogService>();
        _service = new MediaAssetService(
            _mockMediaRepo.Object,
            _mockMapper.Object,
            _mockBlob.Object,
            _mockListingRepo.Object,
            _mockLogService.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnDto_WhenSuccess()
    {
        var mockFile = new Mock<IFormFile>();
        var files = new List<IFormFile> { mockFile.Object };

        var mediaType = MediaType.Picture;
        int listingcaseId = 1;
        int mediaId = 1;
        string userId = "user-123";
        string url = "xxx-xxx";

        var mediaAsset = new MediaAsset
        {
            Id = mediaId,
            MediaType = mediaType,
            MediaUrl = url,
        };

        var responseDto = new MediaAssetResponseDto
        {
            Id = mediaId,
            MediaType = mediaType,
            MediaUrl = url,
        };

        _mockBlob.Setup(b => b.UploadAsync(It.IsAny<IFormFile>())).ReturnsAsync(url);
        _mockMediaRepo.Setup(m => m.CreateAsync(It.IsAny<MediaAsset>())).ReturnsAsync(mediaAsset);
        _mockListingRepo.Setup(l => l.ExistsAsync(listingcaseId, userId)).ReturnsAsync(true);
        _mockMapper.Setup(m => m.Map<MediaAssetResponseDto>(mediaAsset)).Returns(responseDto);

        var result = await _service.CreateAsync(files, mediaType, listingcaseId, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(mediaType, result.First().MediaType);
        Assert.Equal(mediaId, result.First().Id);
        Assert.Equal(url, result.First().MediaUrl);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenUploadMultipleNonPictureFiles()
    {
        var mockFile1 = new Mock<IFormFile>();
        var mockFile2 = new Mock<IFormFile>();

        var files = new List<IFormFile> { mockFile1.Object, mockFile2.Object };

        var mediaType = MediaType.Video;

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(files, mediaType, 1, "user-123")
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenListingCaseNotFound()
    {
        var mockFile = new Mock<IFormFile>();
        var files = new List<IFormFile> { mockFile.Object };

        _mockListingRepo
            .Setup(l => l.ExistsAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.CreateAsync(files, MediaType.Picture, 1, "user-123")
        );
    }

    // ── GetAsync ────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAsync_ShouldThrowUnauthorizedAccessException_WhenAdminHasNoAccess()
    {
        _mockListingRepo
            .Setup(l => l.ExistsAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.GetAsync(1, "user-123", "Admin")
        );
    }

    [Fact]
    public async Task GetAsync_ShouldThrowUnauthorizedAccessException_WhenAgentHasNoAccess()
    {
        _mockListingRepo
            .Setup(l => l.IsAssignedToAgentAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.GetAsync(1, "agent-123", "Agent")
        );
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDtos_WhenAdminHasAccess()
    {
        var mediaAssets = CreateSampleMediaAssets();
        var responseDtos = CreateSampleMediaAssetDtos();

        _mockListingRepo.Setup(l => l.ExistsAsync(1, "user-123")).ReturnsAsync(true);
        _mockMediaRepo.Setup(r => r.GetAssetsAsync(1)).ReturnsAsync(mediaAssets);
        _mockMapper
            .Setup(m => m.Map<IEnumerable<MediaAssetResponseDto>>(mediaAssets))
            .Returns(responseDtos);

        var result = await _service.GetAsync(1, "user-123", "Admin");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("url-1", result.First().MediaUrl);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDtos_WhenAgentHasAccess()
    {
        var mediaAssets = CreateSampleMediaAssets();
        var responseDtos = CreateSampleMediaAssetDtos();

        _mockListingRepo.Setup(l => l.IsAssignedToAgentAsync(1, "user-123")).ReturnsAsync(true);
        _mockMediaRepo.Setup(r => r.GetAssetsAsync(1)).ReturnsAsync(mediaAssets);
        _mockMapper
            .Setup(m => m.Map<IEnumerable<MediaAssetResponseDto>>(mediaAssets))
            .Returns(responseDtos);

        var result = await _service.GetAsync(1, "user-123", "Agent");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("url-1", result.First().MediaUrl);
    }

    // ── DeleteAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenMediaNotFound()
    {
        _mockMediaRepo
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((MediaAsset?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync("user-123", 1));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteMedia_WhenSuccess()
    {
        var mediaAsset = new MediaAsset
        {
            Id = 1,
            MediaType = MediaType.Picture,
            MediaUrl = "url-to-delete",
            ListingCaseId = 1,
        };

        _mockMediaRepo.Setup(r => r.GetByIdAsync(1, "user-123")).ReturnsAsync(mediaAsset);
        _mockBlob.Setup(b => b.DeleteAsync("url-to-delete")).Returns(Task.CompletedTask);
        _mockMediaRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
        _mockLogService
            .Setup(l => l.LogCaseHistoryAsync(It.IsAny<Remp.Service.LogModels.CaseHistoryLog>()))
            .Returns(Task.CompletedTask);

        await _service.DeleteAsync("user-123", 1);

        _mockBlob.Verify(b => b.DeleteAsync("url-to-delete"), Times.Once);
        _mockMediaRepo.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}
