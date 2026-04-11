using Scalar.AspNetCore;
using Remp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Remp.Service.Mappers;
using Remp.Service.Interfaces;
using Remp.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Remp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Remp.Repositories.Interfaces;
using Remp.Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Dbcontext
builder.Services.AddDbContext<RempDbContext>(Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("RecamDb")));

// Mappers
builder.Services.AddAutoMapper(cfg => {}, typeof(MappingProfile));

// Scopes
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAgentRepository, AgentRepository>();

// Jwt autentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Identity
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<RempDbContext>();

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
