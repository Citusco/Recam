using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Remp.Models.Entities;
using Remp.Models.Enums;
using Remp.Repositories.Interfaces;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;

namespace Remp.Service.Services;

public class MediaAssetService : IMediaAssetService
{
    private readonly IMediaAssetRepository _mediaAssetRepository;
    private readonly IMapper _mapper;
    private readonly IBlobUploadService _blobService;
    private readonly IListingCaseRepository _listingCaseRepository;

    public MediaAssetService (IMediaAssetRepository repository, IMapper mapper, IBlobUploadService blobUploadService, IListingCaseRepository listingCaseRepository)
    {
        _mediaAssetRepository = repository;
        _mapper = mapper;
        _blobService = blobUploadService;
        _listingCaseRepository = listingCaseRepository;
    }

    public async Task<IEnumerable<CreateMediaAssetResponseDto>> CreateAsync(List<IFormFile> files, MediaType mediaType, int listingCaseId, string userId)
    {
        if (mediaType != MediaType.Picture && files.Count() > 1)
        {
            throw new InvalidOperationException("Only Pictures allow multiple file upload.");
        }
        if (!await _listingCaseRepository.ExistsAsync(listingCaseId, userId))
        {
            throw new KeyNotFoundException("Listing case not found");
        }

        List<CreateMediaAssetResponseDto> responseDtos = [];

        foreach (var f in files)
        {
            string url = await _blobService.UploadAsync(f);
            MediaAsset mediaAsset = new()
            {
                MediaType = mediaType,
                MediaUrl = url,
                ListingCaseId = listingCaseId,
                UserId = userId,
                IsSelect = false,
                IsHero = false,
                IsDeleted = false
            };

            MediaAsset result = await _mediaAssetRepository.CreateAsync(mediaAsset);
            responseDtos.Add(_mapper.Map<CreateMediaAssetResponseDto>(result));
        }

        return responseDtos;
    }

    public async Task<IEnumerable<MediaAssetResponseDto>> GetAsync(int listingCaseId, string userId, string role)
    {
        // Check existence and accessibility.
        bool hasAccess;
        if (role == "Admin")
        {
            hasAccess = await _listingCaseRepository.ExistsAsync(listingCaseId, userId);
        }
        else
        {
            hasAccess = await _listingCaseRepository.IsAssignedToAgentAsync(listingCaseId, userId);
        }
        if (! hasAccess)
        {
            throw new UnauthorizedAccessException("Cannot access listingcase.");
        }

        IEnumerable<MediaAsset> mediaAssets = await _mediaAssetRepository.GetAssetsAsync(listingCaseId);
        IEnumerable<MediaAssetResponseDto> responseDtos = _mapper.Map<IEnumerable<MediaAssetResponseDto>>(mediaAssets);

        return responseDtos;
    }
}
