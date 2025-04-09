using Movies.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using DbContext = Movies.Data.DbContext;

namespace Movies.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../Movies.API")); 
                config.SetBasePath(path)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DbContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("Movies.Migrations")));
            })
            .Build();
        host.Run();
    }
}