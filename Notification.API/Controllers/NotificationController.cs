using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Notification.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly NotificationDbContext _dbContext;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(NotificationDbContext dbContext, ILogger<NotificationController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.Notification>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Models.Notification>>> GetNotifications([FromQuery] bool unreadOnly = false)
    {
        try
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var query = _dbContext.Notifications.AsQueryable();
            
            if (unreadOnly)
            {
                query = query.Where(n => !n.IsRead);
            }

            var notifications = await query
                .OrderByDescending(n => n.CreatedAt)
                .Take(50)
                .ToListAsync();

            return Ok(notifications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications");
            return StatusCode(500, new { message = "An error occurred while retrieving notifications" });
        }
    }

    [HttpPut("{id}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            var notification = await _dbContext.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound(new { message = "Notification not found" });
            }

            notification.IsRead = true;
            notification.Status = "Read";
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Notification marked as read" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification as read");
            return StatusCode(500, new { message = "An error occurred while updating the notification" });
        }
    }

    [HttpPut("read-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkAllAsRead()
    {
        try
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var unreadNotifications = await _dbContext.Notifications
                .Where(n => !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.Status = "Read";
            }

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "All notifications marked as read" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking all notifications as read");
            return StatusCode(500, new { message = "An error occurred while updating notifications" });
        }
    }
} 