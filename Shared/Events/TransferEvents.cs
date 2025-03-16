namespace BankingSystem.Shared.Events;

public class TransferCreatedEvent
{
    public string TransactionId { get; set; }
    public string FromIBAN { get; set; }
    public string ToIBAN { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
} 