using Remp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Remp.Service.DTOs;

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
    }
}
