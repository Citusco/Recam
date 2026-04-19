using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Remp.Models.Entities;
using Remp.DataAccess.Data;

namespace Remp.DataAccess.Seeding;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var dbContext = serviceProvider.GetRequiredService<RempDbContext>();

        // Seed Admin role
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Seed Admin user and PhotographyCompany
        var adminEmail = configuration["AdminSeed:Email"]!;
        var adminPassword = configuration["AdminSeed:Password"]!;
        var companyName = configuration["AdminSeed:CompanyName"]!;

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                IsDeleted = false
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");

                // Create PhotographyCompany record for Admin
                var company = new PhotographyCompany
                {
                    Id = admin.Id,
                    PhotographyCompanyName = companyName
                };
                await dbContext.PhotographyCompanies.AddAsync(company);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
