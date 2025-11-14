using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Filminurk.Models.FavouriteLists
{
    public class FavouriteListIndexImageViewModel
    {
        public Guid ImageID { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; } 
        public string Image { get; set; }
        public Guid? ListID { get; set; }

    }
}
