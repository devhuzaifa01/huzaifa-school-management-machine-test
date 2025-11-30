using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.Teacher;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Infrastructure.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ILogger<AssignmentService> _logger;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IClassRepository _classRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly FileUploadService _fileUploadService;

        public AssignmentService(ILogger<AssignmentService> logger,
            IAssignmentRepository assignmentRepository,
            IClassRepository classRepository,
            IEnrollmentRepository enrollmentRepository,
            ISubmissionRepository submissionRepository,
            IUserRepository userRepository,
            FileUploadService fileUploadService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
            _submissionRepository = submissionRepository ?? throw new ArgumentNullException(nameof(submissionRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(fileUploadService));
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

                if (request.DueDate.Date < DateTime.UtcNow.Date)
                {
                    throw new InvalidOperationException("Assignment due date cannot be in the past");
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

        public async Task<StudentAssignmentDto> GetByIdForStudentAsync(int id, int studentId)
        {
            try
            {
                var assignment = await _assignmentRepository.GetByIdWithClassAsync(id);
                if (assignment is null)
                {
                    throw new InvalidOperationException("Assignment not found");
                }

                var student = await _userRepository.GetByIdAsync(studentId);
                if (student is null)
                {
                    throw new InvalidOperationException("Student not found");
                }

                if (student.Role != UserRole.Student.ToString())
                {
                    throw new InvalidOperationException("Only students can view assignments");
                }

                var isEnrolled = await _enrollmentRepository.IsStudentEnrolledAsync(studentId, assignment.ClassId);
                if (!isEnrolled)
                {
                    throw new InvalidOperationException("You are not enrolled in the class for this assignment");
                }

                var submission = await _submissionRepository.GetByAssignmentIdAndStudentIdAsync(id, studentId);
                string status = submission is not null ? "Submitted" : "Not Submitted";

                StudentAssignmentDto assignmentDto = new()
                {
                    Id = assignment.Id,
                    ClassId = assignment.ClassId,
                    ClassName = assignment.Class?.Name,
                    Title = assignment.Title,
                    Description = assignment.Description,
                    DueDate = assignment.DueDate,
                    CreatedByTeacherId = assignment.CreatedByTeacherId,
                    CreatedByTeacherName = assignment.CreatedByTeacher?.Name,
                    CreatedDate = assignment.CreatedDate,
                    Status = status
                };

                return assignmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while retrieving assignment {id} for student {studentId}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<SubmissionDto> SubmitAssignmentAsync(int assignmentId, IFormFile file, int studentId, string webRootPath)
        {
            string? storedFileName = null;

            try
            {
                var assignment = await _assignmentRepository.GetByIdWithClassAsync(assignmentId);
                if (assignment is null)
                {
                    throw new InvalidOperationException("Assignment not found");
                }

                if (assignment.DueDate.Date < DateTime.UtcNow.Date)
                {
                    throw new InvalidOperationException("Cannot submit assignment. The due date has passed");
                }

                var student = await _userRepository.GetByIdAsync(studentId);
                if (student is null)
                {
                    throw new InvalidOperationException("Student not found");
                }

                if (student.Role != UserRole.Student.ToString())
                {
                    throw new InvalidOperationException("Only students can submit assignments");
                }

                var isEnrolled = await _enrollmentRepository.IsStudentEnrolledAsync(studentId, assignment.ClassId);
                if (!isEnrolled)
                {
                    throw new InvalidOperationException("You are not enrolled in the class for this assignment");
                }

                var existingSubmission = await _submissionRepository.GetByAssignmentIdAndStudentIdAsync(assignmentId, studentId);
                if (existingSubmission is not null)
                {
                    throw new InvalidOperationException("You have already submitted this assignment");
                }

                var (originalFileName, storedFileNameValue, fileUrl) = await _fileUploadService.SaveFileAsync(file, webRootPath);
                storedFileName = storedFileNameValue;

                Submission submission = new()
                {
                    AssignmentId = assignmentId,
                    StudentId = studentId,
                    SubmittedDate = DateTime.UtcNow,
                    FileUrl = fileUrl,
                    OriginalFileName = originalFileName,
                    StoredFileName = storedFileName,
                    Grade = null,
                    GradedByTeacherId = null,
                    Remarks = null,
                    CreatedDate = DateTime.UtcNow
                };

                await _submissionRepository.AddAsync(submission);

                var createdSubmission = await _submissionRepository.GetByIdAsync(submission.Id);

                SubmissionDto submissionDto = new()
                {
                    Id = createdSubmission!.Id,
                    AssignmentId = createdSubmission.AssignmentId,
                    StudentId = createdSubmission.StudentId,
                    StudentName = createdSubmission.Student?.Name,
                    SubmittedDate = createdSubmission.SubmittedDate,
                    FileUrl = createdSubmission.FileUrl,
                    OriginalFileName = createdSubmission.OriginalFileName,
                    StoredFileName = createdSubmission.StoredFileName,
                    Grade = createdSubmission.Grade,
                    GradedByTeacherId = createdSubmission.GradedByTeacherId,
                    GradedByTeacherName = createdSubmission.GradedByTeacher?.Name,
                    Remarks = createdSubmission.Remarks,
                    CreatedDate = createdSubmission.CreatedDate
                };

                return submissionDto;
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(storedFileName))
                {
                    try
                    {
                        _fileUploadService.DeleteFile(storedFileName, webRootPath);
                        _logger.LogWarning($"Deleted uploaded file {storedFileName} due to submission failure for assignment {assignmentId}, student {studentId}");
                    }
                    catch (Exception deleteEx)
                    {
                        _logger.LogError(deleteEx, $"Failed to delete uploaded file {storedFileName} after submission failure");
                    }
                }

                _logger.LogError($"An exception occurred while submitting assignment {assignmentId} for student {studentId}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<SubmissionDto> GradeSubmissionAsync(int submissionId, GradeSubmissionRequest request, int teacherId)
        {
            try
            {
                var submission = await _submissionRepository.GetByIdWithAssignmentAndClassAsync(submissionId);
                if (submission is null)
                {
                    throw new InvalidOperationException("Submission not found");
                }

                if (submission.Assignment is null)
                {
                    throw new InvalidOperationException("Assignment not found for this submission");
                }

                if (submission.Assignment.Class is null)
                {
                    throw new InvalidOperationException("Class not found for this assignment");
                }

                if (submission.Assignment.Class.TeacherId != teacherId)
                {
                    throw new InvalidOperationException("You can only grade submissions for assignments in your classes");
                }

                if (submission.GradedByTeacherId.HasValue)
                {
                    throw new InvalidOperationException("This submission has already been graded");
                }

                var teacher = await _userRepository.GetByIdAsync(teacherId);
                if (teacher is null)
                {
                    throw new InvalidOperationException("Teacher not found");
                }

                if (teacher.Role != UserRole.Teacher.ToString())
                {
                    throw new InvalidOperationException("Only teachers can grade submissions");
                }

                submission.Grade = request.Grade;
                submission.Remarks = request.Remarks;
                submission.GradedByTeacherId = teacherId;
                submission.UpdatedDate = DateTime.UtcNow;

                await _submissionRepository.UpdateAsync(submission);

                var updatedSubmission = await _submissionRepository.GetByIdAsync(submission.Id);

                SubmissionDto submissionDto = new()
                {
                    Id = updatedSubmission!.Id,
                    AssignmentId = updatedSubmission.AssignmentId,
                    StudentId = updatedSubmission.StudentId,
                    StudentName = updatedSubmission.Student?.Name,
                    SubmittedDate = updatedSubmission.SubmittedDate,
                    FileUrl = updatedSubmission.FileUrl,
                    OriginalFileName = updatedSubmission.OriginalFileName,
                    StoredFileName = updatedSubmission.StoredFileName,
                    Grade = updatedSubmission.Grade,
                    GradedByTeacherId = updatedSubmission.GradedByTeacherId,
                    GradedByTeacherName = updatedSubmission.GradedByTeacher?.Name,
                    Remarks = updatedSubmission.Remarks,
                    CreatedDate = updatedSubmission.CreatedDate
                };

                return submissionDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while grading submission {submissionId} by teacher {teacherId}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<SubmissionDto> GetSubmissionByIdForStudentAsync(int submissionId, int studentId)
        {
            try
            {
                var submission = await _submissionRepository.GetByIdAsync(submissionId);
                if (submission is null)
                {
                    throw new InvalidOperationException("Submission not found");
                }

                if (submission.StudentId != studentId)
                {
                    throw new InvalidOperationException("You can only view your own submissions");
                }

                var student = await _userRepository.GetByIdAsync(studentId);
                if (student is null)
                {
                    throw new InvalidOperationException("Student not found");
                }

                if (student.Role != UserRole.Student.ToString())
                {
                    throw new InvalidOperationException("Only students can view submissions");
                }

                SubmissionDto submissionDto = new()
                {
                    Id = submission.Id,
                    AssignmentId = submission.AssignmentId,
                    StudentId = submission.StudentId,
                    StudentName = submission.Student?.Name,
                    SubmittedDate = submission.SubmittedDate,
                    FileUrl = submission.FileUrl,
                    OriginalFileName = submission.OriginalFileName,
                    StoredFileName = submission.StoredFileName,
                    Grade = submission.Grade,
                    GradedByTeacherId = submission.GradedByTeacherId,
                    GradedByTeacherName = submission.GradedByTeacher?.Name,
                    Remarks = submission.Remarks,
                    CreatedDate = submission.CreatedDate
                };

                return submissionDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while retrieving submission {submissionId} for student {studentId}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<SubmissionDto>> GetSubmissionsByStudentIdAsync(int studentId)
        {
            try
            {
                var student = await _userRepository.GetByIdAsync(studentId);
                if (student is null)
                {
                    throw new InvalidOperationException("Student not found");
                }

                if (student.Role != UserRole.Student.ToString())
                {
                    throw new InvalidOperationException("Only students can view their submissions");
                }

                var submissions = await _submissionRepository.GetByStudentIdAsync(studentId);

                List<SubmissionDto> submissionDtos = submissions.Select(submission => new SubmissionDto
                {
                    Id = submission.Id,
                    AssignmentId = submission.AssignmentId,
                    StudentId = submission.StudentId,
                    StudentName = submission.Student?.Name,
                    SubmittedDate = submission.SubmittedDate,
                    FileUrl = submission.FileUrl,
                    OriginalFileName = submission.OriginalFileName,
                    StoredFileName = submission.StoredFileName,
                    Grade = submission.Grade,
                    GradedByTeacherId = submission.GradedByTeacherId,
                    GradedByTeacherName = submission.GradedByTeacher?.Name,
                    Remarks = submission.Remarks,
                    CreatedDate = submission.CreatedDate
                }).ToList();

                return submissionDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while retrieving submissions for student {studentId}. {ex.Message}", ex);
                throw;
            }
        }
    }
}
