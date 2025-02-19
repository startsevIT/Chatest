using ChatestDomain.BusinessEntities.DTOs;

namespace ChatestDomain.BusinessLogic;

public interface IChatRepo
{
    Task CreateAsync(CreateChatDTO dto, Guid userId);
    Task<ReadChatDTO> ReadAndLinkAsync(Guid id, Guid userId);

}
