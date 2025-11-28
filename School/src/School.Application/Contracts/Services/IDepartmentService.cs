using School.Application.Dtos;
using School.Application.Requests.Admin;

namespace School.Application.Contracts.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentDto> CreateAsync(CreateDepartmentRequest request);
        Task<DepartmentDto> GetByIdAsync(int id);
        Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentRequest request);
        Task DeleteAsync(int id);
    }
}
