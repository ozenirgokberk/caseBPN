using MoneyTransfer.API.Core.Application.DTOs;
using MoneyTransfer.API.Infrastructure.ExternalServices;
using BankingSystem.Shared.Events;
using BankingSystem.Shared.MessageBroker;

namespace MoneyTransfer.API.Core.Application.Services;

public class TransferService : ITransferService
{
    private readonly IBankApiClient _bankApiClient;
    private readonly RabbitMQService _rabbitMQService;
    private readonly ILogger<TransferService> _logger;

    public TransferService(
        IBankApiClient bankApiClient,
        RabbitMQService rabbitMQService,
        ILogger<TransferService> logger)
    {
        _bankApiClient = bankApiClient;
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    public async Task<TransferResponseDto> TransferMoneyAsync(TransferRequestDto request)
    {
        try
        {
            // Step 1: Validate source account
            var sourceAccountValidation = await _bankApiClient.ValidateAccountAsync(request.FromIBAN);
            if (!sourceAccountValidation.Success)
            {
                throw new InvalidOperationException($"Source account validation failed: {sourceAccountValidation.Message}");
            }

            // Step 2: Validate target account
            var targetAccountValidation = await _bankApiClient.ValidateAccountAsync(request.ToIBAN);
            if (!targetAccountValidation.Success)
            {
                throw new InvalidOperationException($"Target account validation failed: {targetAccountValidation.Message}");
            }

            // Step 3: Check balance
            var balanceCheck = await _bankApiClient.CheckBalanceAsync(request.FromIBAN, request.Amount);
            if (!balanceCheck.Success)
            {
                throw new InvalidOperationException($"Balance check failed: {balanceCheck.Message}");
            }

            // Step 4: Execute transfer
            var transferResult = await _bankApiClient.TransferMoneyAsync(
                request.FromIBAN,
                request.ToIBAN,
                request.Amount,
                request.Currency);

            if (!transferResult.Success)
            {
                throw new InvalidOperationException($"Transfer failed: {transferResult.Message}");
            }

            if (string.IsNullOrEmpty(transferResult.TransactionId))
            {
                throw new InvalidOperationException("Transfer completed but no transaction ID was generated.");
            }

            // Step 5: Create response
            var response = new TransferResponseDto
            {
                TransactionId = transferResult.TransactionId,
                FromIBAN = request.FromIBAN,
                ToIBAN = request.ToIBAN,
                Amount = request.Amount,
                Currency = request.Currency,
                Status = transferResult.Status ?? "Completed",
                TransactionDate = transferResult.Timestamp,
                Description = request.Description
            };

            // Step 6: Publish transfer event
            var transferEvent = new TransferCreatedEvent
            {
                TransactionId = response.TransactionId,
                FromIBAN = response.FromIBAN,
                ToIBAN = response.ToIBAN,
                Amount = response.Amount,
                Currency = response.Currency,
                Status = response.Status,
                TransactionDate = response.TransactionDate
            };

            await _rabbitMQService.PublishEvent("transfer.created", transferEvent);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during money transfer");
            throw;
        }
    }
} 