using Auth.AuthService.Entity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.AuthService
{
    public class UsersDBContext(DbContextOptions<UsersDBContext> options) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ConfirmationTokenEntity> ConfirmationTokens { get; set; }
        public DbSet<PasswordResetEntity> PasswordResets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();
            //modelBuilder.Entity<ConfirmationTokenEntity>().HasKey(u => u.UserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
