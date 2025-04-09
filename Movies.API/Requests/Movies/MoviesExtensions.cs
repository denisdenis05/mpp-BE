using Movies.API.Requests.PagingAndFiltering;
using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Data.Models;

namespace Movies.API.Requests.Movies;

public static class MoviesExtensions
{
    public static FilterObjectDTO toFilterMoviesDTO(this FilterMoviesRequest request) => new FilterObjectDTO
    {
        OnlyCount = request.OnlyCount,
        Paging = request.Paging.toPagingDTO(),
        Filtering = request.Filtering.toFilterDTOList(),
        Sorting = request.Sorting.toSortingDTO()
    };

    public static DeleteMovieDTO toDeleteMovieDTO(this DeleteMovieRequest request) => new DeleteMovieDTO
    {
        Id = request.Id,
    };

    public static EditMovieDTO toEditMovieDTO(this EditMovieRequest request) => new EditMovieDTO
    {
        Id = request.Id,
        Title = request.Title,
        Writer = request.Writer,
        Director = request.Director,
        Genre = request.Genre,
        MPA = request.MPA,
        Rating = request.Rating,
    };
}