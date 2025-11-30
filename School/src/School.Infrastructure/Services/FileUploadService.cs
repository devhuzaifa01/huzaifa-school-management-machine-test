using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using School.Application.Dtos;

namespace School.Infrastructure.Services
{
    public class FileUploadService
    {
        private readonly FileUploadSettings _fileUploadSettings;

        public FileUploadService(IOptions<FileUploadSettings> fileUploadOptions)
        {
            _fileUploadSettings = fileUploadOptions.Value ?? throw new ArgumentNullException(nameof(fileUploadOptions));
        }

        public async Task<(string OriginalFileName, string StoredFileName, string FileUrl)> SaveFileAsync(IFormFile file, string webRootPath)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required and cannot be empty");
            }

            var originalFileName = file.FileName;
            var fileExtension = Path.GetExtension(originalFileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(fileExtension) || !_fileUploadSettings.AllowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException($"File type not allowed. Allowed types: {string.Join(", ", _fileUploadSettings.AllowedExtensions)}");
            }

            var storedFileName = $"{Guid.NewGuid()}{fileExtension}";
            var submissionsPath = Path.Combine(webRootPath, _fileUploadSettings.SubmissionsFolder);

            if (!Directory.Exists(submissionsPath))
            {
                Directory.CreateDirectory(submissionsPath);
            }

            var filePath = Path.Combine(submissionsPath, storedFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/{_fileUploadSettings.SubmissionsFolder}/{storedFileName}";

            return (originalFileName, storedFileName, fileUrl);
        }

        public void DeleteFile(string storedFileName, string webRootPath)
        {
            if (string.IsNullOrEmpty(storedFileName))
            {
                return;
            }

            var submissionsPath = Path.Combine(webRootPath, _fileUploadSettings.SubmissionsFolder);
            var filePath = Path.Combine(submissionsPath, storedFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}

