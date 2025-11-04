using Filminurk.Core.Domain;

namespace Filminurk.Models.Actors
{
    public class ActorsDetailsViewModel
    {
        public Guid? ActorID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NickName { get; set; }
        public List<Movie>? MoviesActedFor { get; set; }
        // public Guid PortraitID { get; set; }

        // 3 õpilase andmetüübi
        public ActorsFavouriteGenre? FavouriteGenre { get; set; }
        public bool? HasAwards { get; set; }
        public bool? American { get; set; }

        // andmebaasi jaoks
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
