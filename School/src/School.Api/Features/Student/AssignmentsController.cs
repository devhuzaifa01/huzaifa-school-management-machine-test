using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Contracts.Services;
using System.Security.Claims;

namespace School.Api.Features.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AssignmentsController(IAssignmentService assignmentService, IWebHostEnvironment webHostEnvironment)
        {
            _assignmentService = assignmentService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _assignmentService.GetByIdForStudentAsync(id, studentId);
            return Ok(result);
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit(int id, IFormFile file)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
            {
                return Unauthorized("Invalid user information");
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required");
            }

            var webRootPath = _webHostEnvironment.WebRootPath ?? throw new InvalidOperationException("WebRootPath is not configured");
            var result = await _assignmentService.SubmitAssignmentAsync(id, file, studentId, webRootPath);
            return Ok(result);
        }

        [HttpGet("submissions/{id}")]
        public async Task<IActionResult> GetSubmissionById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _assignmentService.GetSubmissionByIdForStudentAsync(id, studentId);
            return Ok(result);
        }
    }
}
