namespace CustomerRegistration.API.Models;

public class Customer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CustomerNumber { get; set; }
    public string IdentityNumber { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool IsActive { get; set; }
} 