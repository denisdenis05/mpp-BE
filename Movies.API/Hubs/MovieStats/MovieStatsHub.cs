using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Movies.Business.Models.Movies.Stats;

namespace Movies.API.Hubs.MovieStats;

[AllowAnonymous]
public class MovieStatsHub : Hub
{
    public async Task SendHistogramData(List<HistogramBin> data) =>
        await Clients.All.SendAsync("ReceiveHistogramData", data);

    public async Task SendPieChartData(List<PieSlice> data) =>
        await Clients.All.SendAsync("ReceivePieChartData", data);

    public async Task SendTopWritersData(List<TopWriter> data) =>
        await Clients.All.SendAsync("ReceiveTopWritersData", data);
}
