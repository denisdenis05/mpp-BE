using Microsoft.AspNetCore.Mvc;
using Movies.API.Requests.Movies;
using Movies.Business.Services.Movies;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{

    private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

    public FilesController()
    {
        if (!Directory.Exists(_uploadFolder))
        {
            Directory.CreateDirectory(_uploadFolder);
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        var filePath = Path.Combine(_uploadFolder, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { file.FileName });
    }

    [HttpGet("all")]
    public IActionResult GetAllFiles()
    {
        var files = Directory.GetFiles(_uploadFolder)
            .Select(Path.GetFileName)
            .ToList();

        return Ok(files);
    }

    [HttpGet("download/{fileName}")]
    public IActionResult Download(string fileName)
    {
        var filePath = Path.Combine(_uploadFolder, fileName);
        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var contentType = "application/octet-stream";
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, contentType, fileName);
    }
}