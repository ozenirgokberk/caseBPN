namespace UserManagement.API.Core.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpirationTime { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public DateTime? LastLoginAt { get; set; }
} 