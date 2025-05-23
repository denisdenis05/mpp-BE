using Movies.API.Requests.Movies;
using Movies.API.Requests.PagingAndFiltering;
using Movies.Business.Models.PagingAndFiltering;

namespace Movies.API.Requests.EventLog;

public static class EventLogExtensions
{
    public static FilterObjectDTO toFilterDTO(this EventLogFilterRequest request) => new FilterObjectDTO
    {
        OnlyCount = request.OnlyCount,
        Paging = request.Paging.toPagingDTO(),
        Filtering = request.Filtering.toFilterDTOList(),
        Sorting = request.Sorting.toSortingDTO()
    };
}