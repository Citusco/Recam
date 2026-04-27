using AutoMapper;
using Moq;
using Remp.Models.Entities;
using Remp.Models.Enums;
using Remp.Repositories.Interfaces;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;
using Remp.Service.LogModels;
using Remp.Service.Services;

namespace Remp.Tests;

public class ListingCaseServiceTests
{
    private readonly Mock<IListingCaseRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogService> _mockLogService;
    private readonly ListingCaseService _service;

    public ListingCaseServiceTests()
    {
        _mockRepo = new Mock<IListingCaseRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogService = new Mock<ILogService>();
        _service = new ListingCaseService(
            _mockRepo.Object,
            _mockMapper.Object,
            _mockLogService.Object
        );
    }

    // CreateListingCaseAsync

    [Fact]
    public async Task CreateListingCaseAsync_ShouldReturnDto_WhenSuccess()
    {
        // Arrange
        var userId = "user-123";
        var requestDto = new CreateListingCaseRequestDto
        {
            Title = "Test Title",
            Description = "Test Description",
            Street = "123 Test St",
            City = "Test City",
            State = "Test State",
        };
        var listingCase = new ListingCase { Id = 1, UserId = userId };
        var responseDto = new ListingCaseResponseDto
        {
            Id = 1,
            Title = "Test Title",
            City = "Test City",
            State = "Test State",
            ListCaseStatus = ListCaseStatus.Created,
        };

        _mockMapper.Setup(m => m.Map<ListingCase>(requestDto)).Returns(listingCase);
        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<ListingCase>())).ReturnsAsync(listingCase);
        _mockMapper.Setup(m => m.Map<ListingCaseResponseDto>(listingCase)).Returns(responseDto);
        _mockLogService
            .Setup(l => l.LogCaseHistoryAsync(It.IsAny<CaseHistoryLog>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateListingCaseAsync(requestDto, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Title", result.Title);
        Assert.Equal("Test City", result.City);
        Assert.Equal("Test State", result.State);
        Assert.Equal(ListCaseStatus.Created, result.ListCaseStatus);
    }

    // GetAsync

    [Fact]
    public async Task GetAsync_ShouldThrowException_WhenListingCaseNotFound()
    {
        // Arrange
        _mockRepo
            .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((ListingCase?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetAsync(1, "user-123", "Admin"));
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDto_WhenListingCaseExists()
    {
        // Arrange
        var listingCase = new ListingCase { Id = 1 };
        var responseDto = new ListingCaseDetailResponseDto
        {
            Id = 1,
            Title = "Test Title",
            City = "Test City",
            State = "Test State",
            Description = "Test Description",
            Street = "123 Test St",
        };

        _mockRepo.Setup(r => r.GetAsync(1, "user-123", "Admin")).ReturnsAsync(listingCase);
        _mockMapper
            .Setup(m => m.Map<ListingCaseDetailResponseDto>(listingCase))
            .Returns(responseDto);

        // Act
        var result = await _service.GetAsync(1, "user-123", "Admin");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Title", result.Title);
        Assert.Equal("Test City", result.City);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal("123 Test St", result.Street);
    }

    // UpdateListingStatus

    [Fact]
    public async Task UpdateListingStatus_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        // Arrange
        _mockRepo
            .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((ListingCase?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateListingStatus(1, "user-123", "Admin")
        );
    }

    [Fact]
    public async Task UpdateListingStatus_ShouldThrowInvalidOperationException_WhenAlreadyDelivered()
    {
        // Arrange
        var listingCase = new ListingCase { Id = 1, ListCaseStatus = ListCaseStatus.Delivered };

        _mockRepo.Setup(r => r.GetAsync(1, "user-123", "Admin")).ReturnsAsync(listingCase);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateListingStatus(1, "user-123", "Admin")
        );
    }

    [Fact]
    public async Task UpdateListingStatus_ShouldReturnDto_WhenSuccess()
    {
        // Arrange
        var listingCase = new ListingCase { Id = 1, ListCaseStatus = ListCaseStatus.Created };
        var responseDto = new ListingCaseResponseDto
        {
            Id = 1,
            Title = "Test Title",
            ListCaseStatus = ListCaseStatus.Pending,
        };

        _mockRepo.Setup(r => r.GetAsync(1, "user-123", "Admin")).ReturnsAsync(listingCase);
        _mockRepo.Setup(r => r.UpdateStatus(It.IsAny<ListingCase>())).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<ListingCaseResponseDto>(listingCase)).Returns(responseDto);
        _mockLogService
            .Setup(l => l.LogCaseHistoryAsync(It.IsAny<CaseHistoryLog>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateListingStatus(1, "user-123", "Admin");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Title", result.Title);
        Assert.Equal(ListCaseStatus.Pending, result.ListCaseStatus);
    }
}
