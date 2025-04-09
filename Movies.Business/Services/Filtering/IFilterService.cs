using Movies.Business.Models.PagingAndFiltering;

namespace Movies.Business.Services.Filtering;

public interface IFilterService<TIn, TOut>
{
    Task<FilterResponse<TOut>> Filter(FilterObjectDTO request, IQueryable<TIn> data);
}
