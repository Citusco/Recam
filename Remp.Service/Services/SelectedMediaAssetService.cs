using AutoMapper;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;
using Remp.Service.LogModels;

namespace Remp.Service.Services;

public class SelectedMediaAssetService : ISelectedMediaAssetService
{
    private readonly IListingCaseRepository _listingCaseRepository;
    private readonly IMediaAssetRepository _mediaRepository;
    private readonly ISelectedMediaAssetRepository _selectedMediaRepository;
    private readonly IMapper _mapper;
    private readonly ILogService _logService;

    public SelectedMediaAssetService(
        IListingCaseRepository listingCaseRepository,
        IMediaAssetRepository mediaAssetRepository,
        ISelectedMediaAssetRepository selectedMediaRepository,
        IMapper mapper,
        ILogService logService
    )
    {
        _listingCaseRepository = listingCaseRepository;
        _mediaRepository = mediaAssetRepository;
        _selectedMediaRepository = selectedMediaRepository;
        _mapper = mapper;
        _logService = logService;
    }

    public async Task<IEnumerable<SelectMediaResponseDto>> CreateAsync(
        int listingCaseId,
        string agentId,
        SelectMediaRequestDto requestDto
    )
    {
        if (requestDto.MediaIds.Count() > 10)
            throw new InvalidOperationException("Cannot select more than 10 medias");

        // Check listing accessibility
        bool hasAccess = await _listingCaseRepository.IsAssignedToAgentAsync(
            listingCaseId,
            agentId
        );
        if (!hasAccess)
        {
            throw new InvalidOperationException("The listing case cannot be accessed.");
        }

        // Validate that all requested media assets exists.
        IEnumerable<MediaAsset> mediaAssets = await _mediaRepository.GetByIdsAsync(
            requestDto.MediaIds
        );
        if (mediaAssets.Count() != requestDto.MediaIds.Count())
        {
            throw new KeyNotFoundException("One or more media assets not found.");
        }
        if (mediaAssets.Any(m => m.ListingCaseId != listingCaseId))
            throw new UnauthorizedAccessException("Media asset does not belong to this listing.");

        IEnumerable<SelectedMedia> selectedMedias = mediaAssets.Select(m => new SelectedMedia
        {
            MediaAssetId = m.Id,
            ListingCaseId = listingCaseId,
            AgentId = agentId,
            SelectedAt = DateTime.Now,
        });

        // Reset all selected medias and update.
        await _selectedMediaRepository.DeleteByListingCaseAsync(listingCaseId, agentId);
        IEnumerable<SelectedMedia> resultMedias = await _selectedMediaRepository.CreateAsync(
            selectedMedias
        );
        IEnumerable<SelectMediaResponseDto> responseDtos = _mapper.Map<
            IEnumerable<SelectMediaResponseDto>
        >(resultMedias);

        await _logService.LogCaseHistoryAsync(
            new CaseHistoryLog
            {
                Event = "SelectedMedia",
                OperatorId = agentId,
                ListingCaseId = listingCaseId,
                Detail = $"Selected {responseDtos.Count()} medias",
            }
        );

        return responseDtos;
    }

    public async Task<IEnumerable<MediaAssetResponseDto>> GetFinalSelectionAsync(
        int listingCaseId,
        string agentId
    )
    {
        bool hasAccess = await _listingCaseRepository.IsAssignedToAgentAsync(
            listingCaseId,
            agentId
        );
        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("The listing case cannot be accessed.");
        }
        IEnumerable<MediaAsset> mediaAssets = await _selectedMediaRepository.GetFinalSelectionAsync(
            listingCaseId,
            agentId
        );
        IEnumerable<MediaAssetResponseDto> responseDtos = _mapper.Map<
            IEnumerable<MediaAssetResponseDto>
        >(mediaAssets);

        return responseDtos;
    }
}
