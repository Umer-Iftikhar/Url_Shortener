using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Url_Shortener.Data;
using Url_Shortener.Repositories.Implementations;
using Url_Shortener.Repositories.Interfaces;
using Url_Shortener.Services.Implementations;
using Url_Shortener.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Database connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, serverVersion);
});

// Hashids — salt from user secrets
var hashidsSalt = builder.Configuration["Hashids:Salt"] ?? throw new InvalidOperationException("Hashids salt not found in configuration");
builder.Services.AddSingleton<IHashids>(_ => new Hashids(hashidsSalt, minHashLength: 6));


// Add services to the container.

builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<IShortUrlService, ShortUrlService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
