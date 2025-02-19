using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;
using ChatestDomain.BusinessEntities.Mapping;
using ChatestDomain.BusinessLogic;
using Microsoft.EntityFrameworkCore;

namespace ChatestInfrastructure.Storage.Repos;

public class ChatRepo : IChatRepo
{
    public async Task CreateAsync(CreateChatDTO dto, Guid userId)
    {
        using SQLiteDbContext db = new ();

        User user = await db.Users.FindAsync(userId) ?? throw new Exception("Not found User");

        await db.Chats.AddAsync(dto.Map(user));
        await db.SaveChangesAsync();
    }

    public async Task<ReadChatDTO> ReadAndLinkAsync(Guid id, Guid userId)
    {
        using SQLiteDbContext db = new();

        Chat chat = await db.Chats
            .Include(x => x.Messages)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == id) 
            ?? throw new Exception("Not found chat");

        if(chat.Users.Find(x => x.Id == userId) == null) {
            User user = await db.Users.FindAsync(userId) ?? throw new Exception("User not found");
            chat.Users.Add(user);
            await db.SaveChangesAsync();
        }

        return chat.Map(chat.Messages, chat.Users);
    }
}
