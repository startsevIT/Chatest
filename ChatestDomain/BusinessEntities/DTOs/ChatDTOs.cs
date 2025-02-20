namespace ChatestDomain.BusinessEntities.DTOs;

// DTO для создания нового чата
public record CreateChatDTO(
    string Name // Название чата
);

// DTO для чтения информации о чате с сообщениями и пользователями
public record ReadChatDTO(
    string Name, // Название чата
    List<ReadMessageDTO> Messages, // Список сообщений в чате
    List<ReadForChatUserDTO> Users // Список пользователей в чате
);

// DTO для списка чатов (используется, например, в списке доступных чатов)
public record ReadForListChatDTO(
    Guid Id, // Уникальный ID чата
    string Name // Название чата
);
