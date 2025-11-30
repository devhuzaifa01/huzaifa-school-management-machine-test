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
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _assignmentService.CreateAsync(request, teacherId);
            return Ok(result);
            
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetByClassId(int classId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _assignmentService.GetByClassIdAsync(classId, teacherId);
            return Ok(result);
        }

        [HttpPost("{id}/grade")]
        public async Task<IActionResult> GradeSubmission(int id, [FromBody] GradeSubmissionRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int teacherId))
            {
                return Unauthorized("Invalid user information");
            }

            var result = await _assignmentService.GradeSubmissionAsync(id, request, teacherId);
            return Ok(result);
        }
    }
}
