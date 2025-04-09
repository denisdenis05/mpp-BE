using Microsoft.Extensions.DependencyInjection;
using Movies.Business.Services.Example;
using Movies.Business.Services.Movies;

namespace Movies.Business;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>(); 
    }
}