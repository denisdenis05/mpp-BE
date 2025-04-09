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
        var message = await _service.GetAllMovies(request.toFilterMoviesDTO());
        
        return Ok(message);
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteMovie([FromBody] DeleteMovieRequest request)
    {
        await _service.DeleteMovie(request.toDeleteMovieDTO());
        
        return Ok();
    }
    
    [HttpPost("edit")]
    public async Task<IActionResult> EditMovie([FromBody] EditMovieRequest request)
    {
        await _service.EditMovie(request.toEditMovieDTO());
        
        return Ok();
    }
}