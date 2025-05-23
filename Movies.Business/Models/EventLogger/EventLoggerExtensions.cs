using Movies.Data.Models;

namespace Movies.Business.Models.EventLogger;

public static class EventLoggerExtensions
{
    public static EventCacheResponse toCacheResponse(this EventCache cache) =>
        new EventCacheResponse
        {
            EventCacheId = cache.EventCacheId,
            Token = cache.Token,
            Action = cache.Action,
            Time = cache.Time,
        };
}