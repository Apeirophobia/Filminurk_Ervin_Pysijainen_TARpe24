namespace Filminurk.Models.Movies
{
    public class ImageViewModel
    {
        public Guid ImageID { get; set; }
        public string? FilePath { get; set; }
        public Guid? MovieID { get; set; }
        public bool? isPoster { get; set; }
    }
}
