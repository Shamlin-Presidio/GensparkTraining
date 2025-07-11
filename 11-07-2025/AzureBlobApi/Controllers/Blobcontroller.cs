using AzureBlobApi.DTOs;
using AzureBlobApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDto dto)
        {
            var result = await _blobService.UploadAsync(dto.File);
            return Ok(new { Url = result });
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            var data = await _blobService.DownloadAsync(fileName);
            return File(data, "application/octet-stream", fileName);
        }
    }
}
