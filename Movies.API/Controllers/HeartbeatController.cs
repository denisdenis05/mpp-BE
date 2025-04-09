using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HeartbeatController : ControllerBase
{
    [HttpGet]
    public IActionResult IsAlive()
    {
        return Ok();
    }

}