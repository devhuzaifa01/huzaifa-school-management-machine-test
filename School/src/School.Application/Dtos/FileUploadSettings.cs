namespace School.Application.Dtos
{
    public class FileUploadSettings
    {
        public string[] AllowedExtensions { get; set; } = Array.Empty<string>();
        public string SubmissionsFolder { get; set; } = "submissions";
    }
}

