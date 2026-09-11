namespace JPAzureFunctions.GraphQL.Types;

public class UserInput
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string EbNumber { get; set; } = string.Empty;

    public string EbNumber2 { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}