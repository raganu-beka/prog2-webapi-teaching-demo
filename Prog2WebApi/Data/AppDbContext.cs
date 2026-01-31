using Microsoft.EntityFrameworkCore;
using Prog2WebApi.Models;

namespace Prog2WebApi.Data
{
    // AppDbContext aprakta mūsu datubāzi
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        // Norādam kādas tabulas būs mūsu datubāzē
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Comment> Comment => Set<Comment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base = parent klase
            base.OnModelCreating(modelBuilder);

            // izveidot pareizu sasaisti starp Posts un Users tabulu
            // izveidot FOREIGN KEY
            // -1 ja izdzēsīs lietotāju, automātiski izdzēsīsies posti
            // -2 PostId lauks var būt aizpildīts tikai ar eksistējoša User id
            // Norādam User -> Post relāciju (One-To-Many)
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Norādam User -> Like relāciju (One-To-Many)
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Norādam Post -> Like relāciju (One-To-Many)
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Izdaram tā, lai likes neduplicētos
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.PostId })
                .IsUnique();

            // Norādam User -> Comment relāciju (One-To-Many)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Norādam Post -> Comment relāciju (One-To-Many)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
