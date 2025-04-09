using Movies.Business.Services.Example;
using Microsoft.AspNetCore.Mvc;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ExamplesController : ControllerBase
{
    public IExampleService _service;

    public ExamplesController(IExampleService exampleService)
    {
        _service = exampleService;
    }
    
    [HttpGet("test")]
    public IActionResult GetMessage()
    {
        var message = _service.GetExampleMessage();
        
        return Ok(message);
    }
}
