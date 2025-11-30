using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Contracts.Services;
using School.Application.Requests.Teacher;
using System.Security.Claims;

namespace School.Api.Features.Teacher
{
    [Route("api/teacher/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] CreateNotificationRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            await _notificationService.SendNotificationAsync(request, teacherId);
            return Ok(new { message = "Notification(s) sent successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _notificationService.GetByTeacherIdAsync(teacherId);
            return Ok(result);
        }
    }
}
