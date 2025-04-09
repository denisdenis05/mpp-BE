using Microsoft.Extensions.DependencyInjection;
using Movies.Business.Models.Movies;
using Movies.Business.Services.Filtering;
using Movies.Business.Services.Movies;
using Movies.Data.Models;

namespace Movies.Business;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>(); 
        services.AddScoped(typeof(IFilterService<,>), typeof(FilterService<,>));
        
        services.AddScoped<Func<Movie, MovieResponse>>(provider => 
            p => p.toMovieResponse());

    }
}