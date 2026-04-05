using System;
using Microsoft.AspNetCore.Identity;

namespace Remp.Models.Entities;

public class User : IdentityUser
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set;} = DateTime.Now;
    public ICollection<MediaAsset> MediaAssets;

    public PhotographyCompany PhotographyCompany;
}
