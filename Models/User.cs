namespace JPAzureFunctions.Models
{
    // Represents one record in the Users table.
    public class User
    {
        // Primary Key
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string EbNumber { get; set; } = string.Empty;

        public string? EbNumber2 { get; set; }

        public string Password { get; set; } = string.Empty;

        public DateTime? LastLogin { get; set; }

        // Navigation Properties
        public List<Bill> Bills { get; set; } = new();

        public List<Payment> Payments { get; set; } = new();
    }
}