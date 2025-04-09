
using Movies.Business.Models.Movies;

namespace Movies.API.CONSTANTS;

public static class FilteringConstants
{
    public static Dictionary<string,string> MapAcronymsToOperations = new Dictionary<string,string>()
    {
        {"eq", "=="},
        {"gt",">"},
        {"lt","<"},
        {"ge", ">="},
        {"le", "<="},
        {"ne", "!="},
    };
    
    public static readonly List<string> ValidSortingOrder = new List<string>
    {
        "asc",
        "desc",
        ""
    }; 
    
    public static readonly List<string> ValidSortingFields = new List<string>
    {
        nameof(MovieDTO.Title).ToLower(),
        nameof(MovieDTO.Director).ToLower(),
        nameof(MovieDTO.Writer).ToLower(),
        nameof(MovieDTO.Genre).ToLower(),
        nameof(MovieDTO.MPA).ToLower(),
        nameof(MovieDTO.Rating),
    }; 
    
    public static readonly List<string> ValidFilteringFields = new List<string>
    {
        nameof(MovieDTO.Title).ToLower(),
        nameof(MovieDTO.Director).ToLower(),
        nameof(MovieDTO.Writer).ToLower(),
        nameof(MovieDTO.Genre).ToLower(),
        nameof(MovieDTO.MPA).ToLower(),
        nameof(MovieDTO.Rating),
    }; 
}