namespace MoneyTransfer.API.Infrastructure.ExternalServices;

public interface IBankApiClient
{
    Task<BankApiResponse> ValidateAccountAsync(string iban);
    Task<BankApiResponse> CheckBalanceAsync(string iban, decimal amount);
    Task<TransferResult> TransferMoneyAsync(string fromIban, string toIban, decimal amount, string currency);
}

public class BankApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
}

public class TransferResult : BankApiResponse
{
    public string? TransactionId { get; set; }
    public DateTime Timestamp { get; set; }
} 