namespace UserManagement.API.Shared.Messages;

public class UserCreatedMessage
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
} 