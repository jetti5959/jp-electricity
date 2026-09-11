namespace JPAzureFunctions.Models.DTOs
{
    // Request body for Login API
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}