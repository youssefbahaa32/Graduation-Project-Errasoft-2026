

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<FileUploadResult>> UploadAsync(
            IEnumerable<IFormFile> files,
            string folder,
            CancellationToken cancellationToken = default);

        Task<FileUploadResult?> UploadAsync(
            IFormFile file,
            string folder,
            CancellationToken cancellationToken = default);

        void Delete(string relativePath);
    }
}