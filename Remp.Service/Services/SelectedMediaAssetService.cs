using AutoMapper;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;

namespace Remp.Service.Services;

public class SelectedMediaAssetService : ISelectedMediaAssetService
{
    private readonly IListingCaseRepository _listingCaseRepository;
    private readonly IMediaAssetRepository _mediaRepository;
    private readonly ISelectedMediaAssetRepository _selectedMediaRepository;
    private IMapper _mapper;

    public SelectedMediaAssetService (
        IListingCaseRepository listingCaseRepository,
        IMediaAssetRepository mediaAssetRepository,
        ISelectedMediaAssetRepository selectedMediaRepository,
        IMapper mapper)
    {
        _listingCaseRepository = listingCaseRepository;
        _mediaRepository = mediaAssetRepository;
        _selectedMediaRepository = selectedMediaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SelectMediaResponseDto>> CreateAsync(int listingCaseId, string agentId, SelectMediaRequestDto requestDto)
    {
        if (requestDto.MediaIds.Count() > 10)
            throw new InvalidOperationException("Cannot select more than 10 medias");

        // Check listing accessibility
        bool hasAccess = await _listingCaseRepository.IsAssignedToAgentAsync(listingCaseId, agentId);
        if (!hasAccess)
        {
            throw new InvalidOperationException("The listing case cannot be accessed.");
        }
        
        // Validate that all requested media assets exists.
        IEnumerable<MediaAsset> mediaAssets = await _mediaRepository.GetByIdsAsync(requestDto.MediaIds);
        if (mediaAssets.Count() != requestDto.MediaIds.Count())
        {
            throw new KeyNotFoundException("One or more media assets not found.");
        }
        if (mediaAssets.Any(m => m.ListingCaseId != listingCaseId))
            throw new UnauthorizedAccessException("Media asset does not belong to this listing.");

        IEnumerable<SelectedMedia> selectedMedias = mediaAssets.Select(
            m => new SelectedMedia
            {
                MediaAssetId = m.Id,
                ListingCaseId = listingCaseId,
                AgentId = agentId,
                SelectedAt = DateTime.Now
            }
        );

        // Reset all selected medias and update.
        await _selectedMediaRepository.DeleteByListingCaseAsync(listingCaseId, agentId);
        IEnumerable<SelectedMedia> resultMedias = await _selectedMediaRepository.CreateAsync(selectedMedias);
        IEnumerable<SelectMediaResponseDto> responseDtos = _mapper.Map<IEnumerable<SelectMediaResponseDto>>(resultMedias);

        return responseDtos;
    }
}
