using JPAzureFunctions.Data;
using JPAzureFunctions.Models;
using Microsoft.EntityFrameworkCore;

namespace JPAzureFunctions.GraphQL.Queries;

public class UserQuery
{
    // Get all users
    public async Task<List<User>> GetUsers(AppDbContext db)
    {
        return await db.Users.ToListAsync();
    }

    // Get user by ID
    public async Task<User?> GetUserById(int id, AppDbContext db)
    {
        return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}