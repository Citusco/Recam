using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recam.Models.Entities;

namespace Recam.DataAccess;

public class RecamDbContext : IdentityDbContext<User>
{
    public DbSet<Agent> Agents { get; set; }
    public DbSet<AgentListingCase> AgentListingCases { get; set; }
    public DbSet<ListingCase> ListingCases { get; set; }
    public RecamDbContext (DbContextOptions<RecamDbContext> options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AgentListingCase>().HasKey(p => new {p.AgentId, p.ListingCaseId});
        modelBuilder.Entity<CaseContact>().HasKey(p => p.ContactId);
        // Prevent cycles or multiple cascade delete.
        modelBuilder.Entity<MediaAsset>()
        .HasOne(p => p.User)
        .WithMany(p => p.MediaAssets)
        .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MediaAsset>()
        .HasOne(p => p.ListingCase)
        .WithMany(p => p.MediaAssets)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
