using Microsoft.EntityFrameworkCore;
using JPAzureFunctions.Data;
using JPAzureFunctions.Models;
using JPAzureFunctions.Models.DTOs;
using JPAzureFunctions.Services.Interfaces;

namespace JPAzureFunctions.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    // Register User
    public async Task<(bool Success, string Message)> RegisterAsync(User user)
    {
        if (await _context.Users.AnyAsync(x => x.Email == user.Email))
            return (false, "Email already exists");

        if (await _context.Users.AnyAsync(x => x.EbNumber == user.EbNumber))
            return (false, "EB Number already exists");

        if (!string.IsNullOrEmpty(user.EbNumber2) &&
            await _context.Users.AnyAsync(x => x.EbNumber2 == user.EbNumber2))
            return (false, "EB Number 2 already exists");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (true, "Registration Successful");
    }

    // Login User
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Email == request.Email &&
            x.Password == request.Password);

        if (user == null)
            return null;

        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            EbNumber = user.EbNumber,
            Message = "Login Successful"
        };
    }

    // Update Profile (Next Module)
    public async Task<(bool Success, string Message)> UpdateProfileAsync(int id, User user)
    {
        return (true, "Pending Implementation");
    }

    // Get Profile
    public async Task<User?> GetProfileAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}