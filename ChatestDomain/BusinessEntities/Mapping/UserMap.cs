using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;

namespace ChatestDomain.BusinessEntities.Mapping;

public static class UserMap
{
    public static User Map(this RegisterUserDTO dto)
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            Login = dto.Login,
            Password = dto.Password,
            NickName = dto.NickName,
            Chats = []
        };
    }
    public static ReadForAccountUserDTO Map(this User user,List<Chat> chats)
    {
        return new ReadForAccountUserDTO(
            user.Login, 
            user.NickName,
            [..chats.Select(x => x.Map())]);
    }
    public static ReadForChatUserDTO Map(this User user)
    {
        return new ReadForChatUserDTO(user.NickName);
    }
}
