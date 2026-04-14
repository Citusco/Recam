using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IListingCaseService
{
    Task<ListingCaseResponseDto> CreateListingCaseAsync(CreateListingCaseRequestDto requestDto, string userId);
}
