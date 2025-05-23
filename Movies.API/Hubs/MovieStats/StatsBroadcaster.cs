using Microsoft.AspNetCore.SignalR;
using Movies.Business.Models.Movies.Stats;
using Movies.Data;

namespace Movies.API.Hubs.MovieStats;

public class StatsBroadcaster : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IHubContext<MovieStatsHub> _hubContext;
    private readonly MovieDbContext _movieDbContext;

    public StatsBroadcaster(IHubContext<MovieStatsHub> hubContext, MovieDbContext movieDbContext)
    {
        _hubContext = hubContext;
        _movieDbContext = movieDbContext;
    }

    public async Task BroadcastAll()
    {
        var data = _movieDbContext.Movies.ToList();
        
        var histogram = Enumerable.Range(0, 10).Select(i => new HistogramBin
        {
            Range = $"{i} - {i + 1}",
            Count = data.Count(m => Math.Floor(m.Rating) == i)
        }).ToList();

        
        var pie = new List<PieSlice>
        {
            new PieSlice { Name = "Rating under 5", Value = data.Count(x => x.Rating < 5) },
            new PieSlice { Name = "Rating over 5", Value = data.Count(x => x.Rating >= 5) }
        };

        
        var writers = data.GroupBy(m => m.Writer)
            .Select(g => new TopWriter { Writer = g.Key, Count = g.Count() })
            .OrderByDescending(w => w.Count)
            .Take(5)
            .ToList();

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
