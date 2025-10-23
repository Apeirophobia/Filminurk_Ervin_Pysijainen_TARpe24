using Filminurk.Core.Domain;

namespace Filminurk.Models.Movies
{
    public class MoviesDeleteViewModel
    {
        public Guid? ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? FirstPublished { get; set; }
        public string? Director { get; set; }
        public List<string>? Actors { get; set; }
        public double? CurrentRating { get; set; }
        // public List<UserComment>? Reviews { get; set; }

        // kaasolevate piltide andmeomadused
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();

        /* 3 õpilase valitud andmetüübi */

        public bool? Vulgar { get; set; }
        public Genre? Genre { get; set; }
        public bool? IsOnAdultSwim { get; set; }

        /* Andmebaasi jaoks vajalikud */
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
