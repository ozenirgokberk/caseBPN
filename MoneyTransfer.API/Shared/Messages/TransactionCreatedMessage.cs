namespace MoneyTransfer.API.Shared.Messages;

public class TransactionCreatedMessage
{
    public int TransactionId { get; set; }
    public int FromCustomerId { get; set; }
    public int ToCustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionNumber { get; set; }
    public string Status { get; set; }
} 