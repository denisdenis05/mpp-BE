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

    public async Task DeleteMovie(DeleteMovieDTO request)
    {
        DbContext.Movies = DbContext.Movies.Where(m => m.Id != request.Id).ToList();
    }

    public async Task EditMovie(EditMovieDTO request)
    {
        var movieToEdit = DbContext.Movies.FirstOrDefault(m => m.Id == request.Id);
        
        movieToEdit.Id = request.Id;
        movieToEdit.Title = request.Title;
        movieToEdit.Writer = request.Writer;
        movieToEdit.Director = request.Director;
        movieToEdit.Genre = request.Genre;
        movieToEdit.MPA = request.MPA;
        movieToEdit.Rating = request.Rating;
    }
}