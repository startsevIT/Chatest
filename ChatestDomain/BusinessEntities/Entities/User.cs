namespace ChatestDomain.BusinessEntities.Entities;

// Сущность для пользователя
public class User
{
    public Guid Id { get; set; } // Уникальный ID пользователя
    public string Login { get; set; } // Логин пользователя
    public string Password { get; set; } // Хешированный пароль пользователя
    public string NickName { get; set; } // Никнейм пользователя
    public List<Chat> Chats { get; set; } // Список чатов, в которых состоит пользователь
}
