using Movies.Business.Models.Movies;

namespace Movies.Business.Services.Movies;

public interface IMovieService
{
    List<MovieDTO> GetAllMovies();
}