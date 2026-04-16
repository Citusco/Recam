using Remp.Models.Enums;

namespace Remp.Service.DTOs;

public class CreateMediaAssetResponseDto
{

    public int Id { get; set; }
    public string MediaUrl { get; set; }
    public DateTime UploadedAt { get; set; }
    public MediaType MediaType { get; set; }

}
