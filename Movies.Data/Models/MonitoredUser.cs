namespace Movies.Data.Models;

public class MonitoredUser
{
    public int MonitoredUserId { get; set; }
    public string Token { get; set; }
    public bool Critical  { get; set; }
}