using UserManagement.API.Core.Domain.Entities.Base;

namespace UserManagement.API.Core.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string Role { get; set; } = "User"; // Default role
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
} 