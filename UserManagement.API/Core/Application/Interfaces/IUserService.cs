using UserManagement.API.Core.Application.DTOs.User;

namespace UserManagement.API.Core.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(int id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(CreateUserDto createUserDto);
    Task UpdateAsync(int id, UpdateUserDto updateUserDto);
    Task DeleteAsync(int id);
} 