namespace JPAzureFunctions.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string EbNumber { get; set; } = string.Empty;
        public string? EbNumber2 { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
    }
}