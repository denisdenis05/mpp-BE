using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Data.Models;

namespace Movies.Business.Services.Movies;

public interface IMovieService
{
    Task<FilterResponse<MovieResponse>> GetAllFilteredMovies(FilterObjectDTO request);
    Task<List<MovieResponse>> GetAllMovies();
    Task<List<Endorsement>> GetEndorsements(int movieId);
    Task DeleteMovie(DeleteMovieDTO request);
    Task EditMovie(EditMovieDTO request);
    Task AddMovie(AddMovieDTO request);
    Task<StatsDTO> GetAverages();
}