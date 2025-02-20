using ChatestDomain.BusinessEntities.DTOs;
using ChatestDomain.BusinessEntities.Entities;

namespace ChatestDomain.BusinessEntities.Mapping;

public static class UserMap
{
    // Метод для преобразования RegisterUserDTO в сущность User
    public static User Map(this RegisterUserDTO dto)
    {
        return new User()
        {
            Id = Guid.NewGuid(), // Генерируем уникальный идентификатор для пользователя
            Login = dto.Login, // Логин пользователя из DTO
            Password = dto.Password, // Пароль пользователя (не хешируется здесь)
            NickName = dto.NickName, // Никнейм пользователя из DTO
            Chats = [] // Инициализируем пустой список чатов
        };
    }

    // Метод для преобразования сущности User в ReadForAccountUserDTO
    public static ReadForAccountUserDTO Map(this User user, List<Chat> chats)
    {
        return new ReadForAccountUserDTO(
            user.Login, // Логин пользователя
            user.NickName, // Никнейм пользователя
            [..chats.Select(x => x.Map())] // Преобразуем список чатов в список ReadForListChatDTO
        );
    }

    // Метод для преобразования сущности User в ReadForChatUserDTO
    public static ReadForChatUserDTO Map(this User user)
    {
        return new ReadForChatUserDTO(user.NickName); // Возвращаем DTO с никнеймом пользователя
    }
}
