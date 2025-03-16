using System.ComponentModel.DataAnnotations;

namespace CustomerRegistration.API.Core.Application.DTOs;

public class UpdateCustomerDto
{
    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(11)]
    public string? IdentityNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }
}
 