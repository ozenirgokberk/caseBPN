using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace MoneyTransfer.API.Infrastructure.ExternalServices;

public class BankApiClient : IBankApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<BankApiClient> _logger;

    public BankApiClient(HttpClient httpClient, IConfiguration configuration, ILogger<BankApiClient> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["BankApi:ApiKey"] ?? throw new ArgumentNullException("BankApi:ApiKey configuration is missing");
        _logger = logger;

        // Configure the HttpClient
        _httpClient.BaseAddress = new Uri(configuration["BankApi:BaseUrl"] ?? "https://api.bankapi.com/");
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
    }

    public Task<BankApiResponse> ValidateAccountAsync(string iban)
    {
        // Simulate account validation
        var response = new BankApiResponse
        {
            Success = true,
            Message = "Account is valid",
            Status = "Active"
        };

        return Task.FromResult(response);
    }

    public Task<BankApiResponse> CheckBalanceAsync(string iban, decimal amount)
    {
        // Simulate balance check
        var response = new BankApiResponse
        {
            Success = true,
            Message = "Sufficient balance",
            Status = "Available"
        };

        return Task.FromResult(response);
    }

    public Task<TransferResult> TransferMoneyAsync(string fromIban, string toIban, decimal amount, string currency)
    {
        // Simulate money transfer
        var response = new TransferResult
        {
            Success = true,
            Message = "Transfer successful",
            Status = "Completed",
            TransactionId = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow
        };

        return Task.FromResult(response);
    }
} 