using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly SchoolDbContext _dbContext;

        public AttendanceRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Attendance?> GetByClassIdStudentIdAndDateAsync(int classId, int studentId, DateTime date)
        {
            return await _dbContext.Attendances
                .FirstOrDefaultAsync(a => a.ClassId == classId 
                    && a.StudentId == studentId 
                    && a.Date.Date == date.Date
                    && (a.IsDeleted == null || a.IsDeleted == false));
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _dbContext.Attendances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Include(a => a.MarkedByTeacher)
                .FirstOrDefaultAsync(a => a.Id == id && (a.IsDeleted == null || a.IsDeleted == false));
        }

        public async Task<List<Attendance>> GetByClassIdAsync(int classId)
        {
            return await _dbContext.Attendances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Include(a => a.MarkedByTeacher)
                .Where(a => a.ClassId == classId && (a.IsDeleted == null || a.IsDeleted == false))
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Student!.Name)
                .ToListAsync();
        }

        public async Task<List<Attendance>> GetByStudentIdAsync(int studentId)
        {
            return await _dbContext.Attendances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Include(a => a.MarkedByTeacher)
                .Where(a => a.StudentId == studentId 
                    && (a.IsDeleted == null || a.IsDeleted == false)
                    && (a.Class == null || (a.Class.IsDeleted == null || a.Class.IsDeleted == false)))
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Class!.Name)
                .ToListAsync();
        }

        public async Task<List<Attendance>> GetByStudentIdAndClassIdAsync(int studentId, int classId)
        {
            return await _dbContext.Attendances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Include(a => a.MarkedByTeacher)
                .Where(a => a.StudentId == studentId 
                    && a.ClassId == classId
                    && (a.IsDeleted == null || a.IsDeleted == false)
                    && (a.Class == null || (a.Class.IsDeleted == null || a.Class.IsDeleted == false)))
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<Attendance> AddAsync(Attendance attendance)
        {
            _dbContext.Attendances.Add(attendance);
            await _dbContext.SaveChangesAsync();
            return attendance;
        }
    }
}
