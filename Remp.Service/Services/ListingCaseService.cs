using AutoMapper;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;
using Remp.Models.Enums;

namespace Remp.Service.Services;

public class ListingCaseService : IListingCaseService
{
    private readonly IListingCaseRepository _listingCaseRepository;
    private readonly IMapper _mapper;

    public ListingCaseService(
        IListingCaseRepository listingCaseRepository,
        IMapper mapper)
    {
        _listingCaseRepository = listingCaseRepository;
        _mapper = mapper;
    }

    public async Task<ListingCaseResponseDto> CreateListingCaseAsync(CreateListingCaseRequestDto requestDto, string userId)
    {
        // Create Listing case.
        ListingCase listingCase = _mapper.Map<ListingCase>(requestDto);
        listingCase.CreatedAt = DateTime.Now;
        listingCase.UserId = userId;
        listingCase.IsDeleted = false;
        listingCase.ListCaseStatus = ListCaseStatus.Created;

        // Add to Db
        ListingCase result = await _listingCaseRepository.CreateAsync(listingCase);
        ListingCaseResponseDto responseDto = _mapper.Map<ListingCaseResponseDto>(result);
        return responseDto;
    }
}
