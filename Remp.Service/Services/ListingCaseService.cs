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

    public async Task<IEnumerable<ListingCaseResponseDto>> GetAllAsync(string userId, string role)
    {
        // Get all Listingcases.
        IEnumerable<ListingCase> listingCases = await _listingCaseRepository.GetAllAsync(userId, role);

        IEnumerable<ListingCaseResponseDto> results = _mapper.Map<IEnumerable<ListingCaseResponseDto>>(listingCases);
        return results;
    }
    public async Task<ListingCaseDetailResponseDto> GetAsync(int listingCaseId, string userId, string role)
    {
        ListingCase? listingCase = await _listingCaseRepository.GetAsync(listingCaseId, userId, role);
        // Need to handle null
        if (listingCase == null)
        {
            throw new Exception("Null listing case.");
        }
        ListingCaseDetailResponseDto responseDto = _mapper.Map<ListingCaseDetailResponseDto>(listingCase);
        return responseDto;
    }
}
