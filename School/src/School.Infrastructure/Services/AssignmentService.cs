using Microsoft.Extensions.Logging;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.Teacher;
using School.Domain.Entities;

namespace School.Infrastructure.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ILogger<AssignmentService> _logger;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IClassRepository _classRepository;

        public AssignmentService(ILogger<AssignmentService> logger,
            IAssignmentRepository assignmentRepository,
            IClassRepository classRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
        }

        public async Task<AssignmentDto> CreateAsync(CreateAssignmentRequest request, int teacherId)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAndTeacherIdAsync(request.ClassId, teacherId);
                if (classEntity is null)
                {
                    throw new InvalidOperationException("Class not found or you do not have permission to create assignments for this class");
                }

                if (!classEntity.IsActive)
                {
                    throw new InvalidOperationException("Cannot create assignment for an inactive class");
                }

                Assignment assignment = new()
                {
                    ClassId = request.ClassId,
                    Title = request.Title,
                    Description = request.Description,
                    DueDate = request.DueDate,
                    CreatedByTeacherId = teacherId,
                    CreatedDate = DateTime.UtcNow
                };

                await _assignmentRepository.AddAsync(assignment);

                var createdAssignment = await _assignmentRepository.GetByIdAsync(assignment.Id);

                AssignmentDto assignmentDto = new()
                {
                    Id = createdAssignment!.Id,
                    ClassId = createdAssignment.ClassId,
                    ClassName = createdAssignment.Class?.Name,
                    Title = createdAssignment.Title,
                    Description = createdAssignment.Description,
                    DueDate = createdAssignment.DueDate,
                    CreatedByTeacherId = createdAssignment.CreatedByTeacherId,
                    CreatedByTeacherName = createdAssignment.CreatedByTeacher?.Name,
                    CreatedDate = createdAssignment.CreatedDate
                };

                return assignmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating an assignment for class {request.ClassId}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<AssignmentDto>> GetByClassIdAsync(int classId, int teacherId)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAndTeacherIdAsync(classId, teacherId);
                if (classEntity is null)
                {
                    throw new InvalidOperationException("Class not found or you do not have permission to view assignments for this class");
                }

                var assignments = await _assignmentRepository.GetByClassIdAndTeacherIdAsync(classId, teacherId);

                var assignmentDtos = assignments.Select(a => new AssignmentDto
                {
                    Id = a.Id,
                    ClassId = a.ClassId,
                    ClassName = a.Class?.Name,
                    Title = a.Title,
                    Description = a.Description,
                    DueDate = a.DueDate,
                    CreatedByTeacherId = a.CreatedByTeacherId,
                    CreatedByTeacherName = a.CreatedByTeacher?.Name,
                    CreatedDate = a.CreatedDate
                }).ToList();

                return assignmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while retrieving assignments for class {classId}. {ex.Message}", ex);
                throw;
            }
        }
    }
}
