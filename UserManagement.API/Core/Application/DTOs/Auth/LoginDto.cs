namespace UserManagement.API.Core.Application.DTOs.Auth;

public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? IpAddress { get; set; }
} 