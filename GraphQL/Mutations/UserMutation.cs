using JPAzureFunctions.Data;
using JPAzureFunctions.Models;
using JPAzureFunctions.GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace JPAzureFunctions.GraphQL.Mutations;

public class UserMutation
{
    // Register User
    public async Task<User> RegisterUser(UserInput input, AppDbContext db)
    {
        var user = new User
        {
            FullName = input.FullName,
            Email = input.Email,
            Phone = input.Phone,
            EbNumber = input.EbNumber,
            EbNumber2 = input.EbNumber2,
            Password = input.Password
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return user;
    }

    // Update User
    public async Task<User?> UpdateUser(
        int id,
        string fullName,
        AppDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return null;

        user.FullName = fullName;

        await db.SaveChangesAsync();

        return user;
    }

    // Delete User
    public async Task<bool> DeleteUser(int id, AppDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return false;

        db.Users.Remove(user);

        await db.SaveChangesAsync();

        return true;
    }
}