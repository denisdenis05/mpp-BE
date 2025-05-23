using Microsoft.EntityFrameworkCore;
using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Business.Services.Filtering;
using Movies.Data;
using Movies.Data.Models;

namespace Movies.Business.Services.Movies;

public class MovieService: IMovieService
{
    private IFilterService<Movie, MovieResponse> _filterService;
    private MovieDbContext _movieDbContext;

    public MovieService(IFilterService<Movie, MovieResponse> filterService, MovieDbContext movieDbContext)
    {
        _filterService = filterService;
        _movieDbContext = movieDbContext;
    }
    
    public async Task<FilterResponse<MovieResponse>> GetAllFilteredMovies(FilterObjectDTO request)
    {
        var data = _movieDbContext.Movies.AsQueryable();
        return await _filterService.Filter(request, data);
    }

    public async Task<List<MovieResponse>> GetAllMovies()
    {
        return await _movieDbContext.Movies
            .Select(movie => movie.toMovieResponse())
            .ToListAsync();
    }

    public async Task<List<Endorsement>> GetEndorsements(int movieId)
    {
        var movie = _movieDbContext.Movies
            .Include(movie => movie.Endorsements)
            .Where(movie => movie.Id == movieId)
            .FirstOrDefault();

        return movie.Endorsements.Take(10).ToList();
    }

    public async Task DeleteMovie(DeleteMovieDTO request)
    {
        var movie = await _movieDbContext.Movies.FindAsync(request.Id);
        if (movie is not null)
        {
            _movieDbContext.Movies.Remove(movie);
            await _movieDbContext.SaveChangesAsync();
            
            
        }
    }

    public async Task EditMovie(EditMovieDTO request)
    {
        var movieToEdit = await _movieDbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.Id);
        if (movieToEdit is not null)
        {
            movieToEdit.Title = request.Title;
            movieToEdit.Writer = request.Writer;
            movieToEdit.Director = request.Director;
            movieToEdit.Genre = request.Genre;
            movieToEdit.MPA = request.MPA;
            movieToEdit.Rating = request.Rating;

            await _movieDbContext.SaveChangesAsync();
        }
    }

    public async Task AddMovie(AddMovieDTO request)
    {
        var newMovie = new Movie
        {
            Writer = request.Writer,
            Director = request.Director,
            Genre = request.Genre,
            MPA = request.MPA,
            Rating = request.Rating,
            Title = request.Title
        };

        _movieDbContext.Movies.Add(newMovie);
        await _movieDbContext.SaveChangesAsync();
    }

    public async Task<StatsDTO?> GetAverages()
    {
        var ratings = await _movieDbContext.Movies
            .Select(m => m.Rating)
            .ToListAsync();

        return new StatsDTO
        {
            Max = ratings.Max(),
            Min = ratings.Min(),
            Average = ratings.Average()
        };
    }
}