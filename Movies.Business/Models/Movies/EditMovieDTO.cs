namespace Movies.Business.Models.Movies;

public class EditMovieDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Director { get; set; }
    public string Writer { get; set; }
    public string Genre { get; set; }
    public string MPA { get; set; }
    public double Rating { get; set; }
}