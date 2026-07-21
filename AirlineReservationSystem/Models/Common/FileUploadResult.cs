namespace AirlineReservationSystem.Models.Common
{
    public class FileUploadResult
    {
        public string FileName { get; set; } = null!;

        public string RelativePath { get; set; } = null!;

        public string ContentType { get; set; } = null!;
    }
}
