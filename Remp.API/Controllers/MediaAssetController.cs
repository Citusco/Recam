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
        private readonly IMediaAssetService _mediaService;
        private readonly ISelectedMediaAssetService _selectedMediaService;
        
        public MediaAssetController(
            IMediaAssetService mediaService,
            ISelectedMediaAssetService selectedMediaService)
        {
            _mediaService = mediaService;
            _selectedMediaService = selectedMediaService;
        }

        [HttpPost("{id}/media")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CreateMediaAssetResponseDto>>> CreateAsync(
            [FromRoute] int id,
            [FromForm] List<IFormFile> files,
            [FromForm] MediaType mediaType)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            IEnumerable<CreateMediaAssetResponseDto> responseDtos = await _mediaService.CreateAsync(files, mediaType, id, userId);
            return Ok(responseDtos);
        }

        [HttpGet("{id}/media")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MediaAssetResponseDto>>> GetAsync([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var role = User.FindFirst(ClaimTypes.Role)!.Value;

            IEnumerable<MediaAssetResponseDto> responseDtos = await _mediaService.GetAsync(id, userId, role);
            return Ok(responseDtos);
        }

        [HttpDelete("/api/media/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            await _mediaService.DeleteAsync(userId, id);
            return Ok(new {message = "Media asset deleted successfully."});
        }

        [HttpPut("{id}/selected-media")]
        [Authorize(Roles = "Agent")]
        public async Task<ActionResult<IEnumerable<SelectMediaResponseDto>>> SelectMedia (
            [FromRoute] int id,
            [FromBody] SelectMediaRequestDto requestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            IEnumerable<SelectMediaResponseDto> responseDtos = await _selectedMediaService.CreateAsync(id, userId, requestDto);

            return Ok(responseDtos);
        }
    }
}
