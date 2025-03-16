namespace BankingSystem.Shared.Events;

public class UserRegisteredEvent
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime RegisteredAt { get; set; }
}

public class UserUpdatedEvent
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime UpdatedAt { get; set; }
} 