﻿using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;
using ChatestDomain.BusinessEntities.Mapping;
using ChatestDomain.BusinessLogic;
using Microsoft.EntityFrameworkCore;

namespace ChatestInfrastructure.Storage.Repos;

public class MessageRepo : IMessageRepo
{
    public async Task<Guid> CreateAsync(CreateMessageDTO dto, Guid userId, Guid chatId)
    {
        using SQLiteDbContext db = new();

        User user = await db.Users.FindAsync(userId) ?? throw new Exception("Not found User");
        Chat chat = await db.Chats.FindAsync(chatId) ?? throw new Exception("Not found Chat");

        Message message = dto.Map(chat, user);

        await db.Messages.AddAsync(message);
        await db.SaveChangesAsync();
        return message.Id;
    }

    public async Task<ReadMessageDTO> ReadAsync(Guid id)
    {
        using SQLiteDbContext db = new();

        Message message = await db.Messages
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id) 
            ?? throw new Exception("Not found message");

        return message.Map(message.User);
    }
}
