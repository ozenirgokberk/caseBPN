using UserManagement.API.Core.Application.DTOs.Auth;
using UserManagement.API.Core.Application.DTOs.User;

namespace UserManagement.API.Core.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<bool> ValidateTokenAsync(string token);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    string GenerateJwtToken(UserDto user);
    string GenerateRefreshToken();
    Task<bool> RevokeRefreshTokenAsync(string username);
} 