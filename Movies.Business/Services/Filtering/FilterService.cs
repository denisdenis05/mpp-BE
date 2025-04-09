using Movies.Business.CONSTANTS;
using Movies.Business.Models.PagingAndFiltering;
using System.Linq.Dynamic.Core;

namespace Movies.Business.Services.Filtering;

public class FilterService<TIn, TOut>: IFilterService<TIn, TOut>
{
    private readonly Func<TIn, TOut> _projector;

    public FilterService(Func<TIn, TOut> projector)
    {
        _projector = projector;
    }

    
    public async Task<FilterResponse<TOut>> Filter(FilterObjectDTO request, IQueryable<TIn> data)
    {
        var result = data;
        
        if (request.Filtering.Count != 0)
        {
            foreach (var filter in request.Filtering)
            {
                var field = filter.FieldToFilterBy;
                var value = filter.Value;
                var operation = FilteringConstants.MapAcronymsToOperations[filter.Operation];

                result = result.Where($"{field} {operation} @0", value);
            }
        }

        if (!string.IsNullOrEmpty(request.Sorting.FieldToSortBy))
        {
            result = result.OrderBy($"{request.Sorting.FieldToSortBy} {request.Sorting.Order}");
        }

        var numberFound = result.Count();
        if (request.OnlyCount)
        {
            return new FilterResponse<TOut>
            {
                NumberFound = numberFound,
                NumberRetrieved = numberFound,
                Results = []
            };
        }
        
        result = result
            .Skip(request.Paging.PageSize * (request.Paging.PageNumber - 1))
            .Take(request.Paging.PageSize);
        
        var actualResult = result.ToList().Select(_projector).ToList();
        
        return new FilterResponse<TOut>
        {
            NumberFound = numberFound,
            NumberRetrieved = result.Count(),
            Results = actualResult,
        };
    }
}