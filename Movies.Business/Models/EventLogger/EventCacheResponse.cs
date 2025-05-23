namespace Movies.Business.Models.EventLogger;

public class EventCacheResponse
{
    public int EventCacheId { get; set; }
    public string Token { get; set; }
    public string Action { get; set; }
    public DateTime Time { get; set; }
}