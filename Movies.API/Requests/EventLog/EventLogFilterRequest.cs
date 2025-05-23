using Movies.API.Requests.PagingAndFiltering;

namespace Movies.API.Requests.EventLog;

public class EventLogFilterRequest
{
    public bool OnlyCount { get; set; } = false;
    public PagingRequest Paging { get; set; } = new PagingRequest();
    public List<FilteringRequest> Filtering { get; set; } = new List<FilteringRequest>();
    public SortingRequest Sorting { get; set; } = new SortingRequest();
}