namespace Movies.Business.Models.EventLogger;

public class MonitoredUserResponse
{
    public int MonitoredUserId { get; set; }
    public string Token { get; set; }
    public bool Critical  { get; set; }
}