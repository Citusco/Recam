using System;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;
using Remp.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Remp.Models.Entities;

namespace Remp.Service.Services;

public class PhotographyCompanyService : IPhotographyCompanyService
{
    private readonly IPhotographyCompanyRepository _photographyCompanyRepository;
    private readonly IMapper _mapper;

    public PhotographyCompanyService(
        IPhotographyCompanyRepository photographyCompanyRepository,
        IMapper mapper)
    {
        _photographyCompanyRepository = photographyCompanyRepository;
        _mapper = mapper;
    }
    public async Task<AgentPhotographyCompanyResponseDto> AssignAgentToCompany(string companyId, string agentId)
    {
        bool exists = await _photographyCompanyRepository.ExistsAsync(companyId, agentId);
        if (exists)
            throw new InvalidOperationException("Agent already been assigned to the company.");
        
        AgentPhotographyCompany agentPhotographyCompany = new()
        {
            PhotographyCompanyId = companyId,
            AgentId = agentId  
        };
        await _photographyCompanyRepository.AssignAgentToCompany(agentPhotographyCompany);
        AgentPhotographyCompanyResponseDto responseDto = _mapper.Map<AgentPhotographyCompanyResponseDto>(agentPhotographyCompany);
        return responseDto;
    }
}
