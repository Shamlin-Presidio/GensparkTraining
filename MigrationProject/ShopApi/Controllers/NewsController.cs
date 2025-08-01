using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos;
using ShopApi.Services.Interfaces;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: api/news?page=1&pageSize=2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsDto>>> GetPagedNews([FromQuery] int page = 1, [FromQuery] int pageSize = 2)
        {
            var news = await _newsService.GetPagedNewsAsync(page, pageSize);
            return Ok(news);
        }
    }
}
