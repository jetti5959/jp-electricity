using Microsoft.EntityFrameworkCore;
using JPAzureFunctions.Models;

namespace JPAzureFunctions.Data
{
    /// <summary>
    /// Entity Framework Core database context.
    /// Manages the database connection and entity mappings.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes the database context with configuration options.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Represents the Users table in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Configures entity mappings and database relationships.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base configuration.
            base.OnModelCreating(modelBuilder);

            // Configure the primary key for the User entity.
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        }
    }
}