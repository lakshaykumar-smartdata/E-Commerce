using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class UserServiceDbContext : DbContext
    {
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
