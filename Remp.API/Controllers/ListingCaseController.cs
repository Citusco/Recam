using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingCaseController : ControllerBase
    {
        private readonly IListingCaseService _service;

        public ListingCaseController(IListingCaseService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ListingCaseResponseDto>> CreateListingCase(
            [FromBody] CreateListingCaseRequestDto listingCaseRequestDto
        )
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            ListingCaseResponseDto responseDto = await _service.CreateListingCaseAsync(
                listingCaseRequestDto,
                userId
            );
            return CreatedAtAction(nameof(CreateListingCase), responseDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ListingCaseResponseDto>>> GetAllListingCase()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            IEnumerable<ListingCaseResponseDto> responseDtos = await _service.GetAllAsync(
                userId,
                role
            );
            return Ok(responseDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ListingCaseDetailResponseDto>> GetListingCaseDetails(
            [FromRoute] int id
        )
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            ListingCaseDetailResponseDto responseDto = await _service.GetAsync(id, userId, role);
            return Ok(responseDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ListingCaseDetailResponseDto>> UpdateListingCase(
            [FromRoute] int id,
            [FromBody] UpdateListingCaseRequestDto requestDto
        )
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            ListingCaseDetailResponseDto responseDto = await _service.UpdateAsync(
                id,
                userId,
                requestDto
            );
            return Ok(responseDto);
        }

        [HttpPost("{id}/agent/{agentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AgentListingCaseResponseDto>> AssignAgentToListingAsync(
            [FromRoute] int id,
            [FromRoute] string agentId
        )
        {
            AgentListingCaseResponseDto responseDto = await _service.AssignAgentToListingAsync(
                id,
                agentId
            );
            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteListingCase([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _service.DeleteAsync(id, userId);
            return Ok(new { message = "Listing case deleted successfully." });
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ListingCaseResponseDto>> UpdateListingStatus(
            [FromRoute] int id
        )
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var role = User.FindFirst(ClaimTypes.Role)!.Value;

            ListingCaseResponseDto responseDto = await _service.UpdateListingStatus(
                id,
                userId,
                role
            );
            return Ok(responseDto);
        }
    }
}
