using System;
using Microsoft.EntityFrameworkCore;
using Recam.Models.Entities;

namespace Recam.DataAccess;

public class RecamDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public RecamDbContext (DbContextOptions<RecamDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder moduleBuilder)
    {
        moduleBuilder.Entity<User>().HasKey(p => p.Id);
        moduleBuilder.Entity<User>().Property(p => p.Name).IsRequired().HasMaxLength(50);
    }
}
