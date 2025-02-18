using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;
using ChatestDomain.BusinessEntities.Mapping;
using ChatestDomain.BusinessLogic;
using ChatestInfrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatestInfrastructure.Storage.Repos;

public class UserRepo : IUserRepo
{
    public async Task<string> LoginAsync(LoginUserDTO dto)
    {
        using SQLiteDbContext db = new ();

        User user = await db.Users
            .FirstOrDefaultAsync(x => x.Login == dto.Login) 
            ?? throw new Exception("Not found User");
        if(user.Password != dto.Password)
            throw new Exception("Not correct password");

        return AuthService.CreateToken([
                new Claim("id",user.Id.ToString())
            ]);
    }

    public async Task<ReadForAccountUserDTO> ReadAsync(Guid id)
    {
        using SQLiteDbContext db = new();

        User user = await db.Users
            .Include(x => x.Chats)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("Not found user");

        return user.Map(user.Chats);
    }

    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        using SQLiteDbContext db = new ();

        User? tryUser = await db.Users.FirstOrDefaultAsync(x => x.Login == dto.Login);
        if (tryUser != null)
            throw new Exception("User already exist");

        var hashed = new RegisterUserDTO(dto.Login, AuthService.HashPassword(dto.Password), dto.NickName);

        await db.Users.AddAsync(hashed.Map());
        await db.SaveChangesAsync();
    }
}
