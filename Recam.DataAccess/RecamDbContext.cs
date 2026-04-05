using System;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recam.Models.Entities;

namespace Recam.DataAccess;

public class RecamDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<Agent> Agents { get; set; }
    public DbSet<AgentListingCase> AgentListingCases { get; set; }
    public DbSet<AgentPhotographyCompany> AgentPhotographyCompanies { get; set; }
    public DbSet<CaseContact> CaseContacts { get; set; }
    public DbSet<ListingCase> ListingCases { get; set; }
    public DbSet<MediaAsset> MediaAssets { get; set; }
    public DbSet<PhotographyCompany> PhotographyCompanies { get; set; }

    public RecamDbContext (DbContextOptions<RecamDbContext> options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicitly define Pks and Fks.
        modelBuilder.Entity<AgentListingCase>().HasKey(p => new {p.AgentId, p.ListingCaseId});
        modelBuilder.Entity<CaseContact>().HasKey(p => p.ContactId);
        modelBuilder.Entity<AgentPhotographyCompany>().HasKey(p => new {p.AgentId, p.PhotographyCompanyId});
        modelBuilder.Entity<PhotographyCompany>()
        .HasOne(p => p.User)
        .WithOne(p => p.PhotographyCompany)
        .HasForeignKey<PhotographyCompany>(p => p.Id);
    
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
