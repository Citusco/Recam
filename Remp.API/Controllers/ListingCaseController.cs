using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.Service.DTOs;
using System.Security.Claims;
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
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<ListingCaseResponseDto>> CreateListingCase([FromBody] CreateListingCaseRequestDto listingCaseRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            ListingCaseResponseDto responseDto = await _service.CreateListingCaseAsync(listingCaseRequestDto, userId);
            return CreatedAtAction(nameof(CreateListingCase), responseDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ListingCaseResponseDto>>> GetAllListingCase()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            IEnumerable<ListingCaseResponseDto> responseDtos = await _service.GetAllAsync(userId, role);
            return Ok(responseDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ListingCaseDetailResponseDto>> GetListingCaseDetails([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            ListingCaseDetailResponseDto responseDto = await _service.GetAsync(id, userId, role);
            return Ok(responseDto);
        }
    }
}
