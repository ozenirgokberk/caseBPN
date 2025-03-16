using MoneyTransfer.API.Core.Application.DTOs;

namespace MoneyTransfer.API.Core.Application.Services;

public interface ITransferService
{
    Task<TransferResponseDto> TransferMoneyAsync(TransferRequestDto request);
} 