using ChatestDomain.BusinessEntities.DTOs;

namespace ChatestDomain.BusinessLogic;

// Интерфейс для работы с чатом в репозитории
public interface IChatRepo
{
    // Метод для создания нового чата
    Task CreateAsync(CreateChatDTO dto, Guid userId);

    // Метод для получения чата и привязки пользователя к чату
    Task<ReadChatDTO> ReadAndLinkAsync(Guid id, Guid userId);
}
