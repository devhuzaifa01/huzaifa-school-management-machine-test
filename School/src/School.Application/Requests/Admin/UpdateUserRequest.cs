namespace School.Application.Requests.Admin
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}

