
using FluentValidation;
using Movies.API.Requests.PagingAndFiltering;

namespace Movies.API.Requests.Movies;

public class GetAllMoviesFilteredRequest
{
    public bool OnlyCount { get; set; } = false;
    public PagingRequest Paging { get; set; } = new PagingRequest();
    public List<FilteringRequest> Filtering { get; set; } = new List<FilteringRequest>();
    public SortingRequest Sorting { get; set; } = new SortingRequest();
    
}

public class FilterProblemsRequestValidator<T> : AbstractValidator<GetAllMoviesFilteredRequest>
{
    public FilterProblemsRequestValidator()
    {
        RuleFor(filter => filter.Paging).SetValidator(new PagingRequestValidator());
        RuleFor(filter => filter.Sorting).SetValidator(new SortingRequestValidator());
        RuleForEach(filter => filter.Filtering).SetValidator(new FilteringRequestValidator());
    }
}