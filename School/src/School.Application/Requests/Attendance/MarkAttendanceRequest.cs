namespace School.Application.Requests.Attendance
{
    public class MarkAttendanceRequest
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

