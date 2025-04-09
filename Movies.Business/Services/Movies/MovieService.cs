using Movies.Business.Models.Movies;
using Movies.Data;

namespace Movies.Business.Services.Movies;

public class MovieService: IMovieService
{
    public List<MovieDTO> GetAllMovies()
    {
        return DbContext.Movies.Select(movie => new MovieDTO
        {
            Title = movie.Title,
            Director = movie.Director,
            Genre = movie.Genre,
            MPA = movie.MPA,
            Writer = movie.Writer,
            Rating = movie.Rating,
        });
    }
}