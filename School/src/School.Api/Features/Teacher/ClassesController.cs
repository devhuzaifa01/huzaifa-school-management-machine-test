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
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IEnrollmentService _enrollmentService;

        public ClassesController(IClassService classService, IEnrollmentService enrollmentService)
        {
            _classService = classService;
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClassRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _classService.CreateAsync(request, teacherId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _classService.GetAllAsync(teacherId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _classService.GetByIdAsync(id, teacherId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateClassRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            try
            {
                var result = await _classService.UpdateAsync(request, teacherId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _classService.DeactivateAsync(id);
            return Ok();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            await _classService.ActivateAsync(id);
            return Ok();
        }

        [HttpPost("{classId}/enroll")]
        public async Task<IActionResult> EnrollStudent(int classId, [FromBody] EnrollStudentRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _enrollmentService.EnrollStudentAsync(classId, request, teacherId);
            return Ok(result);
        }

        [HttpGet("{classId}/enrollments")]
        public async Task<IActionResult> GetEnrollments(int classId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _enrollmentService.GetEnrollmentsByClassIdAsync(classId, teacherId);
            return Ok(result);
        }
    }
}
