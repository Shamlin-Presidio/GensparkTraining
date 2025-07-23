using Microsoft.AspNetCore.Mvc;
using TrainingVideoAPI.DTOs;
using TrainingVideoAPI.Interfaces;

namespace TrainingVideoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly IVideoService _service;

    public VideosController(IVideoService service)
    {
        _service = service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] VideoUploadDto dto)
    {
        var result = await _service.UploadVideoAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var videos = await _service.GetAllVideosAsync();
        return Ok(videos);
    }


    [HttpGet("stream/{videoId}")]
    public async Task<IActionResult> StreamVideo(int videoId)
    {
        try
        {
            var stream = await _service.GetVideoStreamAsync(videoId);
            return File(stream, "video/mp4");
        }
        catch (FileNotFoundException)
        {
            return NotFound("Video not found.");
        }
    }
}
