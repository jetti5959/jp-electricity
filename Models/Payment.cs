namespace JPAzureFunctions.Models
{
    // Represents one payment made by a customer.
    public class Payment
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key -> Users table
        public int UserId { get; set; }

        // Foreign Key -> Bills table
        public int BillId { get; set; }

        // Amount paid
        public decimal AmountPaid { get; set; }

        // UPI / Card / Cash
        public string PaymentMethod { get; set; } = string.Empty;

        // Payment date
        public DateTime PaymentDate { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;

        public Bill Bill { get; set; } = null!;
    }
}