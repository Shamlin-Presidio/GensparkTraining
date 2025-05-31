using Microsoft.AspNetCore.Mvc;
using BankingApi.Interfaces;
using BankingApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class FAQController : ControllerBase
{
    private readonly IFAQService _faqService;

    public FAQController(IFAQService faqService)
    {
        _faqService = faqService;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] FAQDto dto)
    {
        var response = await _faqService.GetAnswerAsync(dto.Question);
        return Content(response, "application/json");
    }
}
