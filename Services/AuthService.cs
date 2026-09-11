/*
------------------------------------------------------------
SOLID Review - AuthService
------------------------------------------------------------

S - Single Responsibility Principle (SRP)
✔ Yes
AuthService has a single responsibility: handling user authentication
business logic such as Register, Login, and Profile operations.

O - Open/Closed Principle (OCP)
✔ Mostly
New authentication features can be added by extending the service
without changing the existing code that uses it.

L - Liskov Substitution Principle (LSP)
✔ Yes
AuthService implements IAuthService, so it can be replaced with
any other implementation of the same interface without affecting callers.

I - Interface Segregation Principle (ISP)
✔ Yes
AuthFunctions depends on the IAuthService interface instead of the
concrete AuthService class, reducing coupling.

D - Dependency Inversion Principle (DIP)
⚠ Partially
AuthFunctions depends on the IAuthService abstraction (good),
but AuthService directly depends on AppDbContext.
Using a repository interface (for example, IUserRepository)
would fully satisfy Dependency Inversion.

------------------------------------------------------------
*/


using Microsoft.EntityFrameworkCore;
using JPAzureFunctions.Data;
using JPAzureFunctions.Models;
using JPAzureFunctions.Models.DTOs;
using JPAzureFunctions.Services.Interfaces;

namespace JPAzureFunctions.Services;

/// <summary>
/// Handles authentication and user-related business logic.
/// Implements the IAuthService interface for dependency injection.
/// </summary>
public class AuthService : IAuthService
{
    // Database context used to access user data.
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes the authentication service with the database context.
    /// </summary>
    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Registers a new user after validating duplicate email and EB numbers.
    /// </summary>
    public async Task<(bool Success, string Message)> RegisterAsync(User user)
    {
        // Check if the email already exists.
        if (await _context.Users.AnyAsync(x => x.Email == user.Email))
            return (false, "Email already exists");

        // Check if the primary EB number already exists.
        if (await _context.Users.AnyAsync(x => x.EbNumber == user.EbNumber))
            return (false, "EB Number already exists");

        // Check if the secondary EB number already exists (if provided).
        if (!string.IsNullOrEmpty(user.EbNumber2) &&
            await _context.Users.AnyAsync(x => x.EbNumber2 == user.EbNumber2))
            return (false, "EB Number 2 already exists");

        // Save the new user to the database.
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (true, "Registration Successful");
    }

    /// <summary>
    /// Validates user login credentials and updates the last login time.
    /// </summary>
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        // Find the user with matching email and password.
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Email == request.Email &&
            x.Password == request.Password);

        // Return null if the credentials are invalid.
        if (user == null)
            return null;

        // Update the user's last successful login time.
        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Return login response with user details.
        return new LoginResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            EbNumber = user.EbNumber,
            Message = "Login Successful"
        };
    }

    /// <summary>
    /// Updates the user's profile.
    /// Placeholder implementation for the next module.
    /// </summary>
    public async Task<(bool Success, string Message)> UpdateProfileAsync(int id, User user)
    {
        return (true, "Pending Implementation");
    }

    /// <summary>
    /// Retrieves a user's profile using their unique ID.
    /// </summary>
    public async Task<User?> GetProfileAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}





