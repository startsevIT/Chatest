using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;

namespace ChatestDomain.BusinessEntities.Mapping;

public static class MessageMap
{
    public static Message Map(this CreateMessageDTO dto, Chat chat, User user)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Text = dto.Text,
            Chat = chat,
            User = user,
            DateTime = DateTime.Now
        };
    }
    public static ReadMessageDTO Map(this Message message, User user)
    {
        return new ReadMessageDTO(message.Text, user.NickName, message.DateTime);
    }
}
