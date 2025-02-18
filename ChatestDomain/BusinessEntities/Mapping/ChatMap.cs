using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;

namespace ChatestDomain.BusinessEntities.Mapping;

public static class ChatMap
{
    public static Chat Map(this CreateChatDTO dto, User user)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Messages = [],
            Users = [user],
        };
    }
    public static ReadChatDTO Map(this Chat chat,List<Message> messages, List<User> users)
    {
        return new ReadChatDTO(
            chat.Name,
            [..messages.Select(x => x.Map(x.User))],
            [..users.Select(x => x.Map())]);
    }
    public static ReadForListChatDTO Map(this Chat chat)
    {
        return new ReadForListChatDTO(chat.Id, chat.Name);
    }
}
