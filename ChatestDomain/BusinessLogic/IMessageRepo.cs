﻿using ChatestDomain.BusinessEntities.DTOs;

namespace ChatestDomain.BusinessLogic;

// Интерфейс для работы с сообщениями в репозитории
public interface IMessageRepo
{
    Task<Guid> CreateAsync(CreateMessageDTO dto, Guid userId, Guid chatId);
    // Метод для создания нового сообщения
    Task CreateAsync(CreateMessageDTO dto, Guid userId, Guid chatId);

    // Метод для получения сообщения по его идентификатору
    Task<ReadMessageDTO> ReadAsync(Guid id);
}
