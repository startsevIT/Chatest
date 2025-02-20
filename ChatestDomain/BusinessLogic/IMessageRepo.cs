using ChatestDomain.BusinessEntities.DTOs;

namespace ChatestDomain.BusinessLogic;

public interface IMessageRepo
{
    Task<Guid> CreateAsync(CreateMessageDTO dto, Guid userId, Guid chatId);
    Task<ReadMessageDTO> ReadAsync(Guid id);
}
