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
    public async Task<ListingCaseDetailResponseDto> UpdateAsync(int listingCaseId, string userId, UpdateListingCaseRequestDto requestDto)
    {
        // Map DTO to a new entity (no Id, UserId, CreatedAt)
        ListingCase updatedData = _mapper.Map<ListingCase>(requestDto);

        ListingCase result = await _listingCaseRepository.UpdateAsync(listingCaseId, userId, updatedData);
        return _mapper.Map<ListingCaseDetailResponseDto>(result);
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

    public async Task DeleteAsync(int listingCaseId, string userId)
    {
        await _listingCaseRepository.DeleteAsync(listingCaseId, userId);
    }
    
    public async Task<ListingCaseResponseDto> UpdateListingStatus(int listingCaseId, string userId, string role)
    {
        // Get Listingcase and check accessibility.
        ListingCase? listingCase = await _listingCaseRepository.GetAsync(listingCaseId, userId, role);
        if (listingCase == null)
            throw new KeyNotFoundException("Listing case not found.");
        
        // Status can only move forward, reverting is not allowed.
        if (listingCase.ListCaseStatus == ListCaseStatus.Delivered)
            throw new InvalidOperationException("Cannot change delivered status");
        await _listingCaseRepository.UpdateStatus(listingCase);
        ListingCaseResponseDto responseDto = _mapper.Map<ListingCaseResponseDto>(listingCase);
        return responseDto;
    }

    public async Task<AgentListingCaseResponseDto> AssignAgentToListingAsync(int listingCaseId, string agentId)
    {
        AgentListingCase agentListingCase = new()
        {
            ListingCaseId = listingCaseId,
            AgentId = agentId
        };
        
        await _listingCaseRepository.AssignAgentToListingAsync(agentListingCase);
        var responseDto = _mapper.Map<AgentListingCaseResponseDto>(agentListingCase);
        return responseDto;
    }
}
