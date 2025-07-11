using Microsoft.EntityFrameworkCore;
using UsernameValidationService.Models;

namespace UsernameValidationService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraint on Username
            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Configure unique constraint on AccountId
            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => u.AccountId)
                .IsUnique();
        }
    }
} 