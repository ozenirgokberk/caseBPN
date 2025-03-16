namespace CustomerRegistration.API.Shared.Messages;

public class CustomerRegisteredMessage
{
    public int CustomerId { get; set; }
    public int UserId { get; set; }
    public string CustomerNumber { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime RegisterDate { get; set; }
} 