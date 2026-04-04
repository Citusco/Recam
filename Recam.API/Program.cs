using Scalar.AspNetCore;
using Recam.DataAccess;
using Microsoft.EntityFrameworkCore;
using Recam.API.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RecamDbContext>(Options =>
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
