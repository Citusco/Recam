using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Remp.API.Middleware;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;
using Remp.Repositories.Repositories;
using Remp.Service.Interfaces;
using Remp.Service.Mappers;
using Remp.Service.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Dbcontext
builder.Services.AddDbContext<RempDbContext>(Options =>
    Options.UseSqlServer(builder.Configuration.GetConnectionString("RecamDb"))
);

// Mappers
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

// Scopes
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAgentRepository, AgentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IListingCaseRepository, ListingCaseRepository>();
builder.Services.AddScoped<IListingCaseService, ListingCaseService>();
builder.Services.AddScoped<IBlobUploadService, BlobUploadService>();
builder.Services.AddScoped<IMediaAssetRepository, MediaAssetRepository>();
builder.Services.AddScoped<IMediaAssetService, MediaAssetService>();
builder.Services.AddScoped<ISelectedMediaAssetRepository, SelectedMediaAssetRepository>();
builder.Services.AddScoped<ISelectedMediaAssetService, SelectedMediaAssetService>();
builder.Services.AddScoped<IPhotographyCompanyRepository, PhotographyCompanyRepository>();
builder.Services.AddScoped<IPhotographyCompanyService, PhotographyCompanyService>();

// Identity
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<RempDbContext>();

// Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Jwt autentication
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
        };
    });

// Controllers
builder.Services.AddControllers();

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed database
using (var scope = app.Services.CreateScope())
{
    await Remp.DataAccess.Seeding.DbSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();
