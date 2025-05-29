using Microsoft.AspNetCore.Mvc;
using BankingApi.Interfaces;
using BankingApi.Models.DTOs;

namespace BankingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountCreateDto dto)
        {
            var account = await _service.CreateAccount(dto);
            return Ok(account);
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> Transaction([FromBody] TransactionDto dto)
        {
            var result = await _service.PerformTransaction(dto);
            return Ok(result);
        }
    }
}