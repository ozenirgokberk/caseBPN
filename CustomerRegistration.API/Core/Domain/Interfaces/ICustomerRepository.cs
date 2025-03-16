using CustomerRegistration.API.Core.Domain.Entities;

namespace CustomerRegistration.API.Core.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeCustomerId = null);
    Task<bool> IdentityNumberExistsAsync(string identityNumber, int? excludeCustomerId = null);
} 