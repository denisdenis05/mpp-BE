using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Requests.EventLog;
using Movies.API.Requests.Movies;
using Movies.Business.Services.Caching;
using Movies.Business.Services.Movies;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class EventLogController : ControllerBase
{
    
    private readonly IEventCacheService _service;
    
    public EventLogController(IEventCacheService service)
    {
        _service = service;
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost("filter")]
    public async Task<IActionResult> GetFiltered([FromBody] EventLogFilterRequest request)
    {
        var message = await _service.GetAllFiltered(request.toFilterDTO());

        return Ok(message);
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getAllMonitoredUsers")]
    public async Task<IActionResult> GetAllMonitored()
    {
        var message = await _service.GetAllMonitored();

        return Ok(message);
    }
}