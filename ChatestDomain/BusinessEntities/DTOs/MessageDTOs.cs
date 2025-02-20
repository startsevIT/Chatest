namespace ChatestDomain.BusinessEntities.DTOs;

// DTO для создания нового сообщения
public record CreateMessageDTO(
    string Text // Текст сообщения
);

// DTO для чтения сообщения с дополнительными данными
public record ReadMessageDTO(
    string Text, // Текст сообщения
    string UserNickName, // Никнейм пользователя
    DateTime DateTime // Время отправки сообщения
);
