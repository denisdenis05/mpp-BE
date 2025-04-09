
using FluentValidation;

namespace Movies.API.Requests.PagingAndFiltering;

public class FilterRequest<T>
{
    public bool OnlyCount { get; set; } = false;
    public PagingRequest Paging { get; set; } = new PagingRequest();
    public List<T> Filtering { get; set; } = new List<T>();
    public SortingRequest Sorting { get; set; } = new SortingRequest();
    
}

public class FilterProblemsRequestValidator : AbstractValidator<T>
{
    public FilterProblemsRequestValidator()
    {
        RuleFor(filterProblems => filterProblems.Paging).SetValidator(new PagingRequestValidator());
        RuleFor(filterProblems => filterProblems.Sorting).SetValidator(new SortingRequestValidator());
        RuleForEach(filterProblems => filterProblems.Filtering).SetValidator(new FilteringRequestValidator());
    }
}