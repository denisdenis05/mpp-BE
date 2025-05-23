using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Movies.Business.Models.EventLogger;
using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Business.Services.Filtering;
using Movies.Data;
using Movies.Data.Models;

namespace Movies.Business.Services.Caching;

public class EventCacheService: IEventCacheService
{
    private MovieDbContext _movieDbContext;
    private IFilterService<EventCache, EventCacheResponse> _filterService;

    public EventCacheService(MovieDbContext dbContext, IFilterService<EventCache, EventCacheResponse> filterService)
    {
        _movieDbContext = dbContext;
        _filterService = filterService;
    }
    
    public async Task LogAction(string action, string userToken)
    {
        _movieDbContext.EventCaches
            .Add(new EventCache
            {
                Action = action,
                Token = userToken,
                Time = DateTime.UtcNow,
            });
        await _movieDbContext.SaveChangesAsync();
    }
    
    public async Task<FilterResponse<EventCacheResponse>> GetAllFiltered(FilterObjectDTO request)
    {
        var data = _movieDbContext.EventCaches.AsQueryable();
        return await _filterService.Filter(request, data);
    }

    public async Task<List<MonitoredUserResponse>> GetAllMonitored()
    {
        return _movieDbContext.MonitoredUsers
            .Select(user => new MonitoredUserResponse
                {
                  MonitoredUserId  = user.MonitoredUserId,
                  Token = user.Token,
                  Critical = user.Critical,
                })
            .ToList();
    }

    public async Task<bool> IsAttacker(string userToken)
    {
        var username = GetUsernameFromJwt(userToken);
        
        return _movieDbContext.MonitoredUsers
            .Where(user => user.Token == username)
            .Select(user => user.Critical == true)
            .Any();
    }
    
    private string GetUsernameFromJwt(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
        
            if (!handler.CanReadToken(token))
            {
                return null;
            }
        
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        
            var usernameClaim = jsonToken?.Claims.FirstOrDefault(c => 
                c.Type == ClaimTypes.Name);

            return usernameClaim?.Value;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}