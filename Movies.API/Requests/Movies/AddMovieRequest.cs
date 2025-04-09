using FluentValidation;
using Movies.API.CONSTANTS;
using Movies.Business.CONSTANTS;

namespace Movies.API.Requests.Movies;

public class AddMovieRequest
{
    public string Title { get; set; }
    public string Director { get; set; }
    public string Writer { get; set; }
    public string Genre { get; set; }
    public string MPA { get; set; }
    public double Rating { get; set; }
}
public class AddMovieRequestValidator : AbstractValidator<AddMovieRequest>
{
    public AddMovieRequestValidator()
    {
        RuleFor(request => request.Title).NotEmpty().Must(title=> title.Length is > 0 and <= 50);
        RuleFor(request => request.Writer).NotEmpty().Must(title=> title.Length is > 0 and <= 50);
        RuleFor(request => request.Genre).NotEmpty().Must(title=> title.Length is > 0 and <= 50);
        RuleFor(request => request.Director).NotEmpty().Must(title=> title.Length is > 0 and <= 50);
        RuleFor(request => request.MPA)
            .Must(field => Constants.ValidMpaRatings.Contains(field.ToLower()));
    }
}
