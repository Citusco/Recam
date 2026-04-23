using System;
using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IUserService
{
    Task<AgentDetailsDto> GetUserAsync (string id);
}
