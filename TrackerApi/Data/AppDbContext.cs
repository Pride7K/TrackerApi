using Microsoft.EntityFrameworkCore;

using System.Reflection.Emit;
using TrackerApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TrackerApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TvShow> TvShows { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TvShow>()
                .HasMany(c => c.Episodes)
                .WithOne(e => e.TvShow).IsRequired();

            modelBuilder.Entity<TvShow>()
                .HasMany(c => c.Actors)
                .WithMany(e => e.TvShows);
        }
    }
}
