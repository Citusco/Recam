using Scalar.AspNetCore;
using Remp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Remp.Service.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RempDbContext>(Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("RecamDb")));

builder.Services.AddAutoMapper(cfg => {}, typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.Run();
