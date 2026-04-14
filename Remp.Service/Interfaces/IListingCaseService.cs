using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IListingCaseService
{
    Task<ListingCaseResponseDto> CreateListingCaseAsync(CreateListingCaseRequestDto requestDto, string userId);
    Task<IEnumerable<ListingCaseResponseDto>> GetAllAsync(string userId, string role);
}
