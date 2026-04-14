using System;
using Microsoft.AspNetCore.Identity;
using Remp.Service.DTOs;
using Remp.Models.Entities;
using AutoMapper;
using Remp.Repositories.Repositories;
using Remp.Repositories.Interfaces;
using Remp.Service.Interfaces;

namespace Remp.Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IAgentRepository _agentRepository;
    private readonly IMapper _mapper;

    public UserService (
        UserManager<User> userManager,
        IAgentRepository agentRepository,
        IMapper mapper)
    {
        _userManager = userManager;
        _agentRepository = agentRepository;
        _mapper = mapper;
    }

    public async Task<UserResponseDto> GetUserAsync(string id)
    {
        User? user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new Exception("Invalid User Id");
        }

        Agent agent = await _agentRepository.GetAgentAsync(id);
        UserResponseDto userResponseDto = _mapper.Map<UserResponseDto>(agent);
        userResponseDto.UserName = user.UserName;
        userResponseDto.Email = user.Email;
        return userResponseDto;
    }
}
