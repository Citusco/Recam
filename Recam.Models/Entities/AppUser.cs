using System;
using Microsoft.AspNetCore.Identity;

namespace Recam.Models.Entities;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
}
