using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace School.Api.Features.Teacher
{
    [Route("api/teacher/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        [HttpGet("TestTeacher")]
        public async Task<IActionResult> TestTeacher()
        {
            return Ok();
        }
    }
}
