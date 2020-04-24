using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movies.Domain;
using Movies.Domain.Models;

namespace Movies.Infrastructure.Data
{
    public class MoviesDbContext : IdentityDbContext<ApplicationUser>
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {
            options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieContext;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;
        }

        public MoviesDbContext() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }



        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<BoardPost> BoardPosts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<FavoritedMovie> FavoritedMovies { get; set; }
    }
}
