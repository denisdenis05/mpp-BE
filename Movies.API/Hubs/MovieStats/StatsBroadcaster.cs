using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Movies.Business.Models.Movies.Stats;
using Movies.Data;

namespace Movies.API.Hubs.MovieStats;

public class StatsBroadcaster : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IHubContext<MovieStatsHub> _hubContext;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StatsBroadcaster(IHubContext<MovieStatsHub> hubContext, IServiceScopeFactory serviceScopeFactory)
    {
        _hubContext = hubContext;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task BroadcastAll()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var movieDbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

        var histogram = await movieDbContext.Movies
            .GroupBy(m => Math.Floor(m.Rating))
            .Select(g => new HistogramBin
            {
                Range = $"{g.Key} - {g.Key + 1}",
                Count = g.Count()
            })
            .ToListAsync();

        // Pie chart (under 5 / over 5)
        var under5 = await movieDbContext.Movies.CountAsync(x => x.Rating < 5);
        var over5 = await movieDbContext.Movies.CountAsync(x => x.Rating >= 5);

        var pie = new List<PieSlice>
        {
            new PieSlice { Name = "Rating under 5", Value = under5 },
            new PieSlice { Name = "Rating over 5", Value = over5 }
        };

        var writers = await movieDbContext.Movies
            .GroupBy(m => m.Writer)
            .Select(g => new TopWriter { Writer = g.Key, Count = g.Count() })
            .OrderByDescending(w => w.Count)
            .Take(5)
            .ToListAsync();

        await _hubContext.Clients.All.SendAsync("ReceiveHistogramData", histogram);
        await _hubContext.Clients.All.SendAsync("ReceivePieChartData", pie);
        await _hubContext.Clients.All.SendAsync("ReceiveTopWritersData", writers);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await BroadcastAll();
            await Task.Delay(2500, stoppingToken); 
        }
    }
}

