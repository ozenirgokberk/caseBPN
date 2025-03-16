using System.ComponentModel.DataAnnotations;

namespace CustomerRegistration.API.Core.Domain.Entities;

public class Customer
{
    public int Id { get; set; }

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

    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
} 