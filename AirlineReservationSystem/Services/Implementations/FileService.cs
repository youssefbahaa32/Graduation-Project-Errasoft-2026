

namespace AirlineReservationSystem.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<FileUploadResult?> UploadAsync(
            IFormFile file,
            string folder,
            CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                folder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueName =
                $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var fullPath = Path.Combine(
                uploadsFolder,
                uniqueName);

            await using var stream = new FileStream(
                fullPath,
                FileMode.Create);

            await file.CopyToAsync(stream, cancellationToken);

            return new FileUploadResult
            {
                FileName = uniqueName,
                RelativePath = Path.Combine(folder, uniqueName)
                    .Replace("\\", "/"),
                ContentType = file.ContentType
            };
        }

        public async Task<List<FileUploadResult>> UploadAsync(
            IEnumerable<IFormFile> files,
            string folder,
            CancellationToken cancellationToken = default)
        {
            List<FileUploadResult> result = [];

            foreach (var file in files)
            {
                var uploaded =
                    await UploadAsync(file, folder, cancellationToken);

                if (uploaded != null)
                    result.Add(uploaded);
            }

            return result;
        }

        public void Delete(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            var fullPath = Path.Combine(
                _environment.WebRootPath,
                relativePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}