using Movies.Business.Models.EventLogger;
using Movies.Business.Models.PagingAndFiltering;

namespace Movies.Business.Services.Caching;

public interface IEventCacheService
{
    Task LogAction(string action, string userToken);
    Task<FilterResponse<EventCacheResponse>> GetAllFiltered(FilterObjectDTO request);
    Task<List<MonitoredUserResponse>> GetAllMonitored();
    Task<bool> IsAttacker(string userToken);
}