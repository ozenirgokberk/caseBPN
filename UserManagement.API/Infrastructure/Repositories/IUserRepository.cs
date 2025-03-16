using UserManagement.API.Models;

namespace UserManagement.API.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<bool> ExistsAsync(string username);
    Task<bool> ExistsEmailAsync(string email);
} 