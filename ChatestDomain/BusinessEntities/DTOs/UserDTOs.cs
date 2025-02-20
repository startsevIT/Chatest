namespace ChatestDomain.BusinessEntities.DTOs;

// DTO для регистрации нового пользователя
public record RegisterUserDTO(
    string Login, // Логин пользователя
    string Password, // Пароль (хэш)
    string NickName // Никнейм пользователя
);

// DTO для авторизации пользователя (логин)
public record LoginUserDTO(
    string Login, // Логин пользователя
    string Password // Пароль пользователя
);

// DTO для отображения информации о пользователе в его аккаунте
public record ReadForAccountUserDTO(
    string Login, // Логин пользователя
    string NickName, // Никнейм пользователя
    List<ReadForListChatDTO> Chats // Список чатов, в которых состоит пользователь
);

// DTO для представления пользователя в чате
public record ReadForChatUserDTO(
    string NickName // Никнейм пользователя
);
