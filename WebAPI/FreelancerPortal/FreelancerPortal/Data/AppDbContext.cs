using FreelancerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelancerPortal.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
                
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Skillset> Skillsets { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User entity configuration
            modelBuilder.Entity<User>(entity =>            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PhoneNumber).HasMaxLength(15);

                // Relationships
                entity.HasMany(u => u.Skillsets)
                      .WithOne(s => s.User)
                      .HasForeignKey(s => s.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Hobbies)
                      .WithOne(h => h.User)
                      .HasForeignKey(h => h.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Skillset entity configuration
            modelBuilder.Entity<Skillset>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.SkillName).IsRequired().HasMaxLength(50);
            });

            // Hobby entity configuration
            modelBuilder.Entity<Hobby>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.HobbyName).IsRequired().HasMaxLength(50);
            });
        }
    }
}
