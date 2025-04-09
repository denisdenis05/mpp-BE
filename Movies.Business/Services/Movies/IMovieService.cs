using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;

namespace Movies.Business.Services.Movies;

public interface IMovieService
{
    Task<FilterResponse<MovieResponse>> GetAllMovies(FilterObjectDTO request);
    Task DeleteMovie(DeleteMovieDTO request);
    Task EditMovie(EditMovieDTO request);
}