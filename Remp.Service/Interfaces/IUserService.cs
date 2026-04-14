using System;
using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IUserService
{
    public Task<UserResponseDto> GetUserAsync (string id);
}
