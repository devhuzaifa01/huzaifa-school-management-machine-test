using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Contracts.Services;
using System.Security.Claims;

namespace School.Api.Features.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendance()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _attendanceService.GetAttendanceByStudentIdAsync(studentId);
            return Ok(result);
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetAttendanceByClassId(int classId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _attendanceService.GetAttendanceByStudentIdAndClassIdAsync(studentId, classId);
            return Ok(result);
        }
    }
}
