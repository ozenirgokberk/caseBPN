namespace UserManagement.API.Core.Application.DTOs.User;

public class UpdateUserDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; }
    public string? Role { get; set; }
} 