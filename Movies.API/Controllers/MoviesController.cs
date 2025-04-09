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
    
    [HttpPost("getAll")]
    public IActionResult GetMessage([FromBody] GetAllMoviesFilteredRequest request)
    {
        var message = _service.GetAllMovies(request.toFilterMoviesDTO());
        
        return Ok(message);
    }
}