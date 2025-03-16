namespace MoneyTransfer.API.Core.Application.DTOs;

public class TransferRequestDto
{
    public string FromIBAN { get; set; }
    public string ToIBAN { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Description { get; set; }
} 