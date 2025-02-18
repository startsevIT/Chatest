namespace ChatestDomain.BusinessEntities.DTOs;

public record CreateChatDTO(
    string Name);
public record ReadChatDTO(
    string Name,
    List<ReadMessageDTO> Messages,
    List<ReadForChatUserDTO> Users);
public record ReadForListChatDTO(
    Guid Id,
    string Name);