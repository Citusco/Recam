using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;
using Remp.Models.Enums;

namespace Remp.API.Controllers
{
    [Route("api/listings")]
    [ApiController]
    public class MediaAssetController : ControllerBase
    {
        private readonly IMediaAssetService _service;
        
        public MediaAssetController(IMediaAssetService service)
        {
            _service = service;
        }

        [HttpPost("{id}/media")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CreateMediaAssetResponseDto>>> CreateAsync(
            [FromRoute] int id,
            [FromForm] List<IFormFile> files,
            [FromForm] MediaType mediaType)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            IEnumerable<CreateMediaAssetResponseDto> responseDtos = await _service.CreateAsync(files, mediaType, id, userId);
            return Ok(responseDtos);
        }
    }
}
