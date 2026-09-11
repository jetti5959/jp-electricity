using Microsoft.EntityFrameworkCore;
using JPAzureFunctions.Models;

namespace JPAzureFunctions.Data
{
    // Database Context
    // This class connects Entity Framework Core with PostgreSQL.
    public class AppDbContext : DbContext
    {
        // Constructor receives database configuration from Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Users table
        public DbSet<User> Users { get; set; }

        // Bills table
        public DbSet<Bill> Bills { get; set; }

        // Payments table
        public DbSet<Payment> Payments { get; set; }

        // Configure Primary Keys and Foreign Keys
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users table Primary Key
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Bills table Primary Key
            modelBuilder.Entity<Bill>()
                .HasKey(b => b.Id);

            // Payments table Primary Key
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            // One User can have Many Bills
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bills)
                .HasForeignKey(b => b.UserId);

            // One User can have Many Payments
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId);

            // One Bill can have Many Payments
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Bill)
                .WithMany()
                .HasForeignKey(p => p.BillId);
        }
    }
}