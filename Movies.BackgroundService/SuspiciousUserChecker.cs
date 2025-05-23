using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Data.Models;

namespace Movies.BackgroundService;

public class SuspiciousUserChecker : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SuspiciousUserChecker> _logger;

    public SuspiciousUserChecker(IServiceScopeFactory scopeFactory, ILogger<SuspiciousUserChecker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

            var oneMinuteAgo = DateTime.UtcNow.AddSeconds(-30);

            var recentEvents = await dbContext.EventCaches
                .Where(e => e.Time > oneMinuteAgo)
                .ToListAsync(stoppingToken);

            var suspiciousGroups = recentEvents
                .Select(e => new {
                    Event = e,
                    Username = GetUsernameFromJwt(e.Token) 
                })
                .GroupBy(item => item.Username)  
                .Select(g => new
                {
                    Username = g.Key,  
                    Count = g.Count(),
                    Actions = g.Select(item => item.Event.Action).Distinct().Count()
                })
                .Where(x => x.Count > 20 || x.Actions < 2)
                .ToList();

            
            foreach (var user in suspiciousGroups)
            {
                var isAttack = user.Count > 100 || user.Actions == 1;

                var existing = await dbContext.MonitoredUsers
                    .FirstOrDefaultAsync(u => u.Token == user.Username, stoppingToken);

                if (existing is null)
                {
                    if(user.Username != null)
                        dbContext.MonitoredUsers.Add(new MonitoredUser
                        {
                            Token = user.Username,
                            Critical = isAttack
                        });
                }
                else
                {
                    existing.Critical |= isAttack;
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
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
