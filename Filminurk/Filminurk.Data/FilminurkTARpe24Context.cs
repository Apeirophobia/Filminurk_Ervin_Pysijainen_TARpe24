using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.Data
{
    public class FilminurkTARpe24Context : DbContext
    {
        public FilminurkTARpe24Context(DbContextOptions<FilminurkTARpe24Context> options) : base(options) { }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<FileToApi> FilesToApi { get; set; }
        public DbSet<UserComment> UserComments { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<FavouriteList> FavouriteLists { get; set; }
        public DbSet<FileToDatabase> FilesToDatabase { get; set; }
    }
}
