using System.Security.Claims;
using EventManagementAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("GetUserCoins/{userId}")]
    public async Task<IActionResult> GetUserCoins(Guid userId)
    {
        try
        {
            int coins = await _walletService.GetCoinsInWallet(userId);
            return Ok(coins);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("TopupCoins/{userId}")]
    public async Task<IActionResult> UpdateCoins(Guid userId, [FromQuery] int coins)
    {
        coins = await _walletService.AddCoinsToWallet(userId, coins, "Topup");
        return Ok(coins);
    }

    [HttpGet("Transactions/{userId}")]
    public async Task<IActionResult> GetTransactionHistory(Guid userId)
    {
        var transactionHistory = await _walletService.GetWalletTransactionHistory(userId);
        return Ok(transactionHistory);
    }

    [HttpGet("Withdraw/List")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> GetEventWithdrawList()
    {
        var organizerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var details = await _walletService.GetEventWithdrawDetails(organizerId);
        return Ok(details);
    }

    [HttpPost("Withdraw/{eventId}")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> WithdrawEventCoins(Guid eventId)
    {
        var organizerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var success = await _walletService.WithdrawEventCoins(eventId, organizerId);
        return Ok(success);
    }
}