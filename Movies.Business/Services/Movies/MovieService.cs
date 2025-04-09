using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Business.Services.Filtering;
using Movies.Data;
using Movies.Data.Models;

namespace Movies.Business.Services.Movies;

public class MovieService: IMovieService
{
    private IFilterService<Movie, MovieResponse> _filterService;

    public MovieService(IFilterService<Movie, MovieResponse> filterService)
    {
        _filterService = filterService;
    }
    
    public async Task<FilterResponse<MovieResponse>> GetAllMovies(FilterObjectDTO request)
    {
        var data = DbContext.Movies.AsQueryable();
        
        return await _filterService.Filter(request, data);
    }
}