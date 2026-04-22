using Remp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Remp.Service.DTOs;
using Remp.Models.Entities;
using Remp.Repositories.Repositories;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotographyCompanyController : ControllerBase
    {
        private readonly IPhotographyCompanyService _photographyCompanyService;

        public PhotographyCompanyController(IPhotographyCompanyService photographyCompanyService)
        {
            _photographyCompanyService = photographyCompanyService;
        }

        [HttpPost("{id}/agent/{agentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddAgentToPhotographyCompany([FromRoute] string id, [FromRoute] string agentId)
        {
            AgentPhotographyCompanyResponseDto responseDto = await _photographyCompanyService.AssignAgentToCompany(id, agentId);

            return Ok(responseDto);
        }

        [HttpGet("agents")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResponseDto<AgentResponseDto>>> GetAllAgents (
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            PagedResponseDto<AgentResponseDto> responseDto = await _photographyCompanyService.GetAllAgentsAsync(page, pageSize);
            return Ok(responseDto);
        }
    }
    
}