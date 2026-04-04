using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recam.Models.Entities;

namespace Recam.DataAccess;

public class RecamDbContext : IdentityDbContext<AppUser>
{
    public RecamDbContext (DbContextOptions<RecamDbContext> options):base(options)
    {
        
    }

    // protected override void OnModelCreating(ModelBuilder moduleBuilder)
    // {
    //     moduleBuilder.Entity<AppUser>().HasKey(p => p.Id);
    //     moduleBuilder.Entity<AppUser>().Property(p => p.Name).IsRequired().HasMaxLength(50);
    // }
}
