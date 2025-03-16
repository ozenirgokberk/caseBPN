namespace MoneyTransfer.API.Models;

public class Transaction
{
    public int Id { get; set; }
    public string TransactionId { get; set; }
    public string FromIBAN { get; set; }
    public string ToIBAN { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string TransactionNumber { get; set; }
} 