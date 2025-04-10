using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Business.Services.Filtering;
using Movies.Data;
using Movies.Data.Models;

namespace Movies.Business.Services.Movies;

public class MovieService: IMovieService
{
    private IFilterService<Movie, MovieResponse> _filterService;
    private DbContext _dbContext;

    public MovieService(IFilterService<Movie, MovieResponse> filterService, DbContext dbContext)
    {
        _filterService = filterService;
        _dbContext = dbContext;
    }
    
    public async Task<FilterResponse<MovieResponse>> GetAllFilteredMovies(FilterObjectDTO request)
    {
        var data = _dbContext.Movies.AsQueryable();
        
        return await _filterService.Filter(request, data);
    }

    public async Task<List<MovieResponse>> GetAllMovies()
    {
        var data = _dbContext.Movies.Select(movie => movie.toMovieResponse()).ToList();

        return data;
    }

    public async Task DeleteMovie(DeleteMovieDTO request)
    {
        _dbContext.Movies = _dbContext.Movies.Where(m => m.Id != request.Id).ToList();
    }

    public async Task EditMovie(EditMovieDTO request)
    {
        var movieToEdit = _dbContext.Movies.FirstOrDefault(m => m.Id == request.Id);
        
        movieToEdit.Id = request.Id;
        movieToEdit.Title = request.Title;
        movieToEdit.Writer = request.Writer;
        movieToEdit.Director = request.Director;
        movieToEdit.Genre = request.Genre;
        movieToEdit.MPA = request.MPA;
        movieToEdit.Rating = request.Rating;
    }

    public async Task AddMovie(AddMovieDTO request)
    {
        _dbContext.Movies.Add(new Movie
        {
            Writer = request.Writer,
            Director = request.Director,
            Genre = request.Genre,
            MPA = request.MPA,
            Rating = request.Rating,
            Title = request.Title
        });
    }

    public async Task<StatsDTO> GetAverages()
    {
        var ratings = _dbContext.Movies
            .Where(m => m.Rating != null)
            .Select(m => m.Rating)
            .ToList();

        if (ratings == null || ratings.Count == 0)
            return null;

        var stats = new StatsDTO
        {
            Max = ratings.Max(),
            Min = ratings.Min(),
            Average = ratings.Average()
        };

        return stats;
    }
}