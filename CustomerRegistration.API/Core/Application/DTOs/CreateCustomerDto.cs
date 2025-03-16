using System.ComponentModel.DataAnnotations;

namespace CustomerRegistration.API.Core.Application.DTOs;

public class CreateCustomerDto
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }

    [StringLength(11)]
    public string? IdentityNumber { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
} 