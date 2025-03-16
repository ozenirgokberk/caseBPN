using UserManagement.API.Core.Application.DTOs.User;

namespace UserManagement.API.Core.Application.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<UserDto> CreateAsync(CreateUserDto userDto);
    Task<UserDto> UpdateAsync(int id, UpdateUserDto userDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<UserDto>> GetAllAsync();
} 