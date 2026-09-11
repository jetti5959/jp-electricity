namespace JPAzureFunctions.Models.DTOs
{
    // Response returned after successful login
    public class LoginResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string EbNumber { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}