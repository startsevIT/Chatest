namespace ChatestDomain.BusinessEntities.Entities;

// Сущность для сообщения
public class Message
{
    public Guid Id { get; set; } // Уникальный ID сообщения
    public string Text { get; set; } // Текст сообщения
    public Chat Chat { get; set; } // Чат, в котором находится это сообщение
    public User User { get; set; } // Пользователь, который отправил сообщение
    public DateTime DateTime { get; set; } // Время отправки сообщения
}





