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
        public async Task<ActionResult<ListingCaseResponseDto>> CreateLisingCase([FromBody] CreateListingCaseRequestDto listingCaseRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            ListingCaseResponseDto responseDto = await _service.CreateListingCaseAsync(listingCaseRequestDto, userId);
            return CreatedAtAction(nameof(CreateLisingCase), responseDto);
        }
    }
}
