using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Requests.Movies;
using Movies.Business.Services.Caching;
using Movies.Business.Services.Movies;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class MoviesController : ControllerBase
{
    public IMovieService _service;
    private readonly IEventCacheService _eventLogger;

    public MoviesController(IMovieService exampleService, IEventCacheService eventLogger)
    {
        _service = exampleService;
        _eventLogger = eventLogger;
    }
    
    private string? GetUserToken()
    {
        return Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetFilteredMovies([FromBody] FilterMoviesRequest request)
    {
        if(await _eventLogger.IsAttacker(GetUserToken()))
            return StatusCode(429, "Too many requests. Possible attack.");
        
        var message = await _service.GetAllFilteredMovies(request.toFilterMoviesDTO());
        await _eventLogger.LogAction("FilterMovies", GetUserToken()!);

        return Ok(message);
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllMovies()
    {
        if(await _eventLogger.IsAttacker(GetUserToken()))
            return StatusCode(429, "Too many requests. Possible attack.");
        
        var message = await _service.GetAllMovies();
        await _eventLogger.LogAction("GetAllMovies", GetUserToken()!);

        return Ok(message);
    }
    
    [HttpGet("get-endorsement")]
    public async Task<IActionResult> GetEndorsements([FromQuery] int movieId)
    {
        if(await _eventLogger.IsAttacker(GetUserToken()))
            return StatusCode(429, "Too many requests. Possible attack.");
        
        var message = await _service.GetEndorsements(movieId);
        await _eventLogger.LogAction("GetEndorsements", GetUserToken()!);

        return Ok(message);
    }
    
    //[Authorize(Roles = "admin")]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteMovie([FromBody] DeleteMovieRequest request)
    {
        if(await _eventLogger.IsAttacker(GetUserToken()))
            return StatusCode(429, "Too many requests. Possible attack.");
        
        await _service.DeleteMovie(request.toDeleteMovieDTO());
        await _eventLogger.LogAction("DeleteMovie", GetUserToken()!);

        return Ok();
    }
    
    [HttpPatch("edit")]
    public async Task<IActionResult> EditMovie([FromBody] EditMovieRequest request)
    {
        if(await _eventLogger.IsAttacker(GetUserToken()))
            return StatusCode(429, "Too many requests. Possible attack.");
        
        await _service.EditMovie(request.toEditMovieDTO());
        await _eventLogger.LogAction("EditMovie", GetUserToken()!);

        return Ok();
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddMovie([FromBody] AddMovieRequest request)
    {
        if(await _eventLogger.IsAttacker(GetUserToken()))
            return StatusCode(429, "Too many requests. Possible attack.");
        
        await _service.AddMovie(request.toAddMovieDTO());
        await _eventLogger.LogAction("AddMovie", GetUserToken()!);

        return Ok();
    }
    
    [HttpGet("get-averages")]
    public async Task<IActionResult> GetAverages()
    {
        if(await _eventLogger.IsAttacker(GetUserToken()))
            return StatusCode(429, "Too many requests. Possible attack.");
        
        var averages = await _service.GetAverages();
        await _eventLogger.LogAction("GetAverages", GetUserToken()!);

        return Ok(averages);
    }
}