using Microsoft.Extensions.DependencyInjection;
using Movies.Business.Models.EventLogger;
using Movies.Business.Models.Movies;
using Movies.Business.Services.Authentication;
using Movies.Business.Services.Caching;
using Movies.Business.Services.Filtering;
using Movies.Business.Services.Movies;
using Movies.Data.Models;

namespace Movies.Business;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>(); 
        services.AddScoped<IAuthService, AuthService>(); 
        services.AddScoped<IEventCacheService, EventCacheService>(); 
        services.AddScoped(typeof(IFilterService<,>), typeof(FilterService<,>));
        
        services.AddScoped<Func<Movie, MovieResponse>>(provider => 
            p => p.toMovieResponse());
        services.AddScoped<Func<EventCache, EventCacheResponse>>(provider => 
            p => p.toCacheResponse());
    }
}