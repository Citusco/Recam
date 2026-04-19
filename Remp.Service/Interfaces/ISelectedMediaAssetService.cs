using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface ISelectedMediaAssetService
{
    Task<IEnumerable<SelectMediaResponseDto>> CreateAsync(int listingCaseId, string agentId, SelectMediaRequestDto requestDto);
}