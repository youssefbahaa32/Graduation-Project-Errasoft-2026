public abstract class BaseImage : AuditableEntity
{
    public string ImageUrl { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;
}