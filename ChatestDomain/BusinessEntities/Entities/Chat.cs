namespace ChatestDomain.BusinessEntities.Entities;

// Сущность для чата
public class Chat
{
    public Guid Id { get; set; } // Уникальный ID чата
    public string Name { get; set; } // Название чата
    public List<Message> Messages { get; set; } // Список сообщений, связанных с этим чатом
    public List<User> Users { get; set; } // Список пользователей, состоящих в чате
}
