using System;
using Microsoft.AspNetCore.Identity;

namespace Recam.Models.Entities;

public class User : IdentityUser
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set;} = DateTime.Now;
}
