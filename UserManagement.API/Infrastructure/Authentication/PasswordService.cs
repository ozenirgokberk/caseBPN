using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace UserManagement.API.Infrastructure.Authentication;

public static class PasswordService
{
    public const int MinimumLength = 8;
    public const int MaximumLength = 128;

    public static (byte[] hash, byte[] salt) HashPassword(string password)
    {
        using var hmac = new HMACSHA512();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (hash, salt);
    }

    public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }

    public static (bool IsValid, string[] Errors) ValidatePassword(string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("Password cannot be empty.");
            return (false, errors.ToArray());
        }

        if (password.Length < MinimumLength)
            errors.Add($"Password must be at least {MinimumLength} characters long.");

        if (password.Length > MaximumLength)
            errors.Add($"Password cannot be longer than {MaximumLength} characters.");

        if (!Regex.IsMatch(password, "[A-Z]"))
            errors.Add("Password must contain at least one uppercase letter.");

        if (!Regex.IsMatch(password, "[a-z]"))
            errors.Add("Password must contain at least one lowercase letter.");

        if (!Regex.IsMatch(password, "[0-9]"))
            errors.Add("Password must contain at least one number.");

        if (!Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]"))
            errors.Add("Password must contain at least one special character (!@#$%^&*(),.?\":{}|<>).");

        return (errors.Count == 0, errors.ToArray());
    }
} 