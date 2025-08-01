using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsManagementController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsManagementController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: api/NewsManagement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsResponseDto>>> GetAll()
        {
            var news = await _newsService.GetAllNewsAsync();
            return Ok(news);
        }

        // GET: api/NewsManagement/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsResponseDto>> GetById(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
                return NotFound();

            return Ok(news);
        }

        // POST: api/NewsManagement
        [HttpPost]
        public async Task<ActionResult<NewsResponseDto>> Create([FromBody] NewsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdNews = await _newsService.CreateNewsAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdNews.NewsId }, createdNews);
        }

        // PUT: api/NewsManagement/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] NewsUpdateDto dto)
        {
            if (id != dto.NewsId)
                return BadRequest("News ID mismatch");
            
            var updatedNews = await _newsService.UpdateNewsAsync(id, dto);
            if (updatedNews == null)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/NewsManagement/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _newsService.DeleteNewsAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // GET: api/NewsManagement/export/csv
        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportToCSV()
        {
            var csvContent = await _newsService.ExportToCsvAsync();

            // return File(System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv", $"NewsListing_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
            return File(csvContent, "text/csv", $"NewsListing_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");

        }

        // GET: api/NewsManagement/export/excel
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var excelBytes = await _newsService.ExportToExcelAsync();
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"NewsListing_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx");
        }
    }
}
