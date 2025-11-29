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
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost]
        public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _attendanceService.MarkAttendanceAsync(request, teacherId);
            return Ok(result);
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetAttendanceHistory(int classId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _attendanceService.GetAttendanceHistoryAsync(classId, teacherId);
            return Ok(result);
        }
    }
}
