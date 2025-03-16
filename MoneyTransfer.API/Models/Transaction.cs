namespace MoneyTransfer.API.Models;

public class Transaction
{
    public int Id { get; set; }
    public int FromCustomerId { get; set; }
    public int ToCustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string TransactionNumber { get; set; }
} 