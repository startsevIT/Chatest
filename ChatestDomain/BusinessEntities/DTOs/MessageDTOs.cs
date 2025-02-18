namespace ChatestDomain.BusinessEntities.DTOs;

public record CreateMessageDTO(
    string Text);
public record ReadMessageDTO(
    string Text,
    string UserNickName, 
    DateTime DateTime);
