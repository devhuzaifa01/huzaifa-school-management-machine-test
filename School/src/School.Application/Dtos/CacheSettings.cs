namespace School.Application.Dtos
{
    public class CacheSettings
    {
        public int CourseExpirySeconds { get; set; } = 60;
        public int DepartmentExpirySeconds { get; set; } = 60;
    }
}

