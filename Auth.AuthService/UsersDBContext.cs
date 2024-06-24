using Auth.AuthService.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auth.AuthService
{
    public class UsersDBContext(DbContextOptions<UsersDBContext> options) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
