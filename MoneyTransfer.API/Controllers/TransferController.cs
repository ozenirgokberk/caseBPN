using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTransfer.API.Core.Application.DTOs;
using MoneyTransfer.API.Core.Application.Services;

namespace MoneyTransfer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransferController : ControllerBase
{
    private readonly ITransferService _transferService;
    private readonly ILogger<TransferController> _logger;

    public TransferController(ITransferService transferService, ILogger<TransferController> logger)
    {
        _transferService = transferService;
        _logger = logger;
    }

    [HttpPost("send")]
    [ProducesResponseType(typeof(TransferResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TransferResponseDto>> SendMoney([FromBody] TransferRequestDto request)
    {
        try
        {
            var result = await _transferService.TransferMoneyAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid transfer operation");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing transfer");
            return StatusCode(500, new { message = "An error occurred while processing the transfer" });
        }
    }
} 