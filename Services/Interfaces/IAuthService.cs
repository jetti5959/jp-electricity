using JPAzureFunctions.Models;
using JPAzureFunctions.Models.DTOs;

namespace JPAzureFunctions.Services.Interfaces;

/// <summary>
/// Defines authentication-related operations.
/// This interface is used to follow Dependency Injection and Interface Segregation principles.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user.
    /// Returns the operation status and a message.
    /// </summary>
    Task<(bool Success, string Message)> RegisterAsync(User user);

    /// <summary>
    /// Authenticates a user using email and password.
    /// Returns user details if login is successful; otherwise returns null.
    /// </summary>
    Task<LoginResponse?> LoginAsync(LoginRequest request);

    /// <summary>
    /// Updates the profile details of an existing user.
    /// Returns the operation status and a message.
    /// </summary>
    Task<(bool Success, string Message)> UpdateProfileAsync(int id, User user);

    /// <summary>
    /// Retrieves the profile information of a user by ID.
    /// </summary>
    Task<User?> GetProfileAsync(int id);
}