namespace UserManagement.API.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }
} 