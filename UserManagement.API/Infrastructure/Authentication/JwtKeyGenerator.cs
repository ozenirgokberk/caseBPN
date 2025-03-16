using System.Security.Cryptography;

namespace UserManagement.API.Infrastructure.Authentication;

public static class JwtKeyGenerator
{
    public static string GenerateSecureKey(int keySizeInBytes = 64) // 64 bytes = 512 bits
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[keySizeInBytes];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public static void PrintNewSecureKey()
    {
        var key = GenerateSecureKey();
        Console.WriteLine("Generated JWT Secret Key (Base64):");
        Console.WriteLine(key);
        Console.WriteLine($"Key length: {key.Length} characters");
    }
} 