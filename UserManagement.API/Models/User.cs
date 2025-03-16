namespace UserManagement.API.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; }
    public DateTime? LastLoginAt { get; set; }
} 