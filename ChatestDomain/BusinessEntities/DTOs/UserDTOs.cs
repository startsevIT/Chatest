namespace ChatestDomain.BusinessEntities.DTOs;

public record RegisterUserDTO(
    string Login,
    string Password,
    string NickName);
public record LoginUserDTO(
    string Login,
    string Password);
public record ReadForAccountUserDTO(
    string Login,
    string NickName,
    List<ReadForListChatDTO> Chats);
public record ReadForChatUserDTO(
    string NickName);