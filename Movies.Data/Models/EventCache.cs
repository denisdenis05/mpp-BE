namespace Movies.Data.Models;

public class EventCache
{
    public int EventCacheId { get; set; }
    public string Token { get; set; }
    public string Action { get; set; }
    public DateTime Time { get; set; }
}