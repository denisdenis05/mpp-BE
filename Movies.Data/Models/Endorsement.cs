namespace Movies.Data.Models;

public class Endorsement
{
    public int EndorsementId { get; set; }
    public string Endorser { get; set; }
    public Movie Movie { get; set; }
    public int MovieId { get; set; }
    public DateTime Date { get; set; }
}