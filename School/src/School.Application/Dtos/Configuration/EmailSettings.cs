namespace School.Application.Dtos.Configuration
{
    public class EmailSettings
    {
        public string SMTPServer { get; set; } = string.Empty;
        public int SMTPPort { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
    }
}

