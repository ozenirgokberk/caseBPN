namespace Notification.API.Models;

public class Notification
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
    public string Data { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
} 