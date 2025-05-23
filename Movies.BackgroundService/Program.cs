using Microsoft.EntityFrameworkCore;
using Movies.BackgroundService;
using Movies.Data;

var builder = Host.CreateApplicationBuilder(args);


var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../Movies.API"));
builder.Configuration
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<SuspiciousUserChecker>();

var host = builder.Build();
host.Run();
