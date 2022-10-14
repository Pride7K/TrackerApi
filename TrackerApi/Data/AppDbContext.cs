using Microsoft.EntityFrameworkCore;

using System.Reflection.Emit;
using TrackerApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TrackerApi.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
        { }


        public DbSet<TvShow> TvShows { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<ActorTvShow> ActorTvShow { get; set; }
        public DbSet<UserTvShowFavorite> UserTvShowFavorite { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActorTvShow>().HasKey(x => new { x.ActorsId, x.TvShowsId });
            modelBuilder.Entity<UserTvShowFavorite>().HasKey(x => new { x.UserId, x.TvShowsId });

            modelBuilder.Entity<ActorTvShow>()
            .HasOne<Actor>(sc => sc.Actor)
            .WithMany(s => s.ActorTvShow)
            .HasForeignKey(sc => sc.ActorsId);

            modelBuilder.Entity<ActorTvShow>().HasOne(pt => pt.TvShow)
            .WithMany(p => p.ActorTvShow)
            .HasForeignKey(pt => pt.TvShowsId);

            modelBuilder.Entity<UserTvShowFavorite>()
            .HasOne<User>(sc => sc.User)
            .WithMany(s => s.UserTvShowFavorite)
            .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserTvShowFavorite>().HasOne(pt => pt.TvShow)
            .WithMany(p => p.UserTvShowFavorite)
            .HasForeignKey(pt => pt.TvShowsId);

            modelBuilder.Entity<TvShow>()
                .HasMany(c => c.Episodes)
                .WithOne(e => e.TvShow).IsRequired();


        }
    }
}
