using FluentValidation;
using Movies.API.CONSTANTS;
using Movies.Business.CONSTANTS;

namespace Movies.API.Requests.PagingAndFiltering;

public class FilteringRequest
{
    public string FieldToFilterBy { get; set; }
    public string Value { get; set; }
    public string Operation { get; set; }
}

public class FilteringRequestValidator : AbstractValidator<FilteringRequest>
{
    public FilteringRequestValidator()
    {
        RuleFor(filtering => filtering.Operation)
            .Must(field => FilteringConstants.MapAcronymsToOperations.ContainsKey(field.ToLower()));

        RuleFor(filtering => filtering.FieldToFilterBy)
            .Must(field => FilteringConstants.ValidFilteringFields.Contains(field.ToLower()));
    }
}