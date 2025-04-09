using Movies.API.Requests.PagingAndFiltering;
using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Data.Models;

namespace Movies.API.Requests.Movies;

public static class MoviesExtensions
{
    public static FilterObjectDTO toFilterMoviesDTO(this GetAllMoviesFilteredRequest request) => new FilterObjectDTO
    {
        OnlyCount = request.OnlyCount,
        Paging = request.Paging.toPagingDTO(),
        Filtering = request.Filtering.toFilterDTOList(),
        Sorting = request.Sorting.toSortingDTO()
    };
}