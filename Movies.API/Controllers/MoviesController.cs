using Microsoft.AspNetCore.Mvc;
using Movies.API.Requests.Movies;
using Movies.Business.Services.Movies;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    public IMovieService _service;

    public MoviesController(IMovieService exampleService)
    {
        _service = exampleService;
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetFilteredMovies([FromBody] FilterMoviesRequest request)
    {
        var message = await _service.GetAllFilteredMovies(request.toFilterMoviesDTO());
        
        return Ok(message);
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllMovies()
    {
        var message = await _service.GetAllMovies();
        
        return Ok(message);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteMovie([FromBody] DeleteMovieRequest request)
    {
        await _service.DeleteMovie(request.toDeleteMovieDTO());
        
        return Ok();
    }
    
    [HttpPatch("edit")]
    public async Task<IActionResult> EditMovie([FromBody] EditMovieRequest request)
    {
        await _service.EditMovie(request.toEditMovieDTO());
        
        return Ok();
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddMovie([FromBody] AddMovieRequest request)
    {
        await _service.AddMovie(request.toAddMovieDTO());
        
        return Ok();
    }
    
    [HttpGet("get-averages")]
    public async Task<IActionResult> GetAverages()
    {
        var averages = await _service.GetAverages();
        
        return Ok(averages);
    }
}