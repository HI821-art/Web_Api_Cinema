using Web_Api_Cinema.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MovieDbContext : IdentityDbContext<User>
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<FavoriteItem> FavoriteItems { get; set; }
    public object Movie { get; internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

       
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies)
            .UsingEntity<Dictionary<string, object>>(
                "ActorMovies",
                j => j.HasOne<Actor>().WithMany().HasForeignKey("ActorId"),
                j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieId"));

        modelBuilder.Entity<Seat>()
    .HasOne(s => s.Customer)
    .WithMany(c => c.Seats)
    .HasForeignKey(s => s.CustomerId)
    .OnDelete(DeleteBehavior.SetNull);



        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(x => new { x.LoginProvider, x.ProviderKey });

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(x => new { x.UserId, x.RoleId });

        modelBuilder.Entity<IdentityUserToken<string>>()
            .HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

   
        modelBuilder.Entity<FavoriteItem>()
            .HasKey(fi => new { fi.UserId, fi.MovieId });


        modelBuilder.Entity<FavoriteItem>()
            .HasOne(fi => fi.User)
            .WithMany(u => u.FavoriteItems)
            .HasForeignKey(fi => fi.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FavoriteItem>()
            .HasOne(fi => fi.Movie)
            .WithMany(m => m.FavoriteItems) 
            .HasForeignKey(fi => fi.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
