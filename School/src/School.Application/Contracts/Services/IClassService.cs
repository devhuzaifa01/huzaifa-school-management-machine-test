using School.Application.Common;
using School.Application.Dtos;
using School.Application.Requests.Teacher;

namespace School.Application.Contracts.Services
{
    public interface IClassService
    {
        Task<ClassDto> CreateAsync(CreateClassRequest request, int teacherId);
        Task<ClassDto> GetByIdAsync(int id, int teacherId);
        Task<List<ClassDto>> GetAllAsync(int teacherId);
        Task<ClassDto> UpdateAsync(UpdateClassRequest request, int teacherId);
        Task DeactivateAsync(int id);
        Task ActivateAsync(int id);
        Task<ClassDto> GetByIdForAdminAsync(int id);
        Task<List<ClassDto>> GetAllForAdminAsync();
        Task<PagedResult<ClassDto>> GetAllPagedForAdminAsync(PagingParameters parameters);
    }
}
