using Movies.Data.Models;

namespace Movies.Business.Models.Movies;

public static class MoviesExtensions
{
    public static MovieResponse toMovieResponse(this Movie movie) => new MovieResponse
    {
        Title = movie.Title,
        Writer = movie.Writer,
        Director = movie.Director,
        MPA = movie.MPA,
        Genre = movie.Genre,
        Rating = movie.Rating,
    };
}