using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Contracts.Services;
using System.Security.Claims;

namespace School.Api.Features.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class ClassesController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public ClassesController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnrolledClasses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _enrollmentService.GetEnrolledClassesByStudentIdAsync(studentId);
            return Ok(result);
        }
    }
}
