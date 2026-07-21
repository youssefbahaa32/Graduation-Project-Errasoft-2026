public class ImageRepository
    : GenericRepository<BaseImage>, IImageRepository
{
    public ImageRepository(ApplicationDbContext context)
        : base(context)
    {   
    }
}