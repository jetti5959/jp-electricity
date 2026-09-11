using JPAzureFunctions.Models;
using JPAzureFunctions.Models.DTOs;

namespace JPAzureFunctions.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string Message)> RegisterAsync(User user);

    Task<LoginResponse?> LoginAsync(LoginRequest request);

    Task<(bool Success, string Message)> UpdateProfileAsync(int id, User user);

    Task<User?> GetProfileAsync(int id);
}