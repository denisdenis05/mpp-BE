using FluentValidation;
using Movies.API.CONSTANTS;

namespace Movies.API.Requests.PagingAndFiltering;

public class SortingRequest
{
    public string FieldToSortBy { get; set; } = string.Empty;
    public string Order { get; set; } = string.Empty;
}

public class SortingRequestValidator : AbstractValidator<SortingRequest>
{
    public SortingRequestValidator()
    {
        RuleFor(sorting => sorting.Order)
            .Must(order => FilteringConstants.ValidSortingOrder.Contains(order.ToLower()));
        
        RuleFor(sorting => sorting.FieldToSortBy)
            .Must(field => FilteringConstants.ValidSortingFields.Contains(field.ToLower()))
            .When(field => field.FieldToSortBy != "");
    }
}