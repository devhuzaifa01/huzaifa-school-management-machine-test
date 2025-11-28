using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Contracts.Services;
using School.Application.Requests.Admin;

namespace School.Api.Features.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
        {
            var result = await _departmentService.CreateAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentRequest request)
        {
            var result = await _departmentService.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);
            return Ok();
        }
    }
}
