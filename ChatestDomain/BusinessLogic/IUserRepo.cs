using ChatestDomain.BusinessEntities.DTOs;

namespace ChatestDomain.BusinessLogic;

// Интерфейс для работы с пользователями в репозитории
public interface IUserRepo
{
    // Метод для регистрации нового пользователя
    Task RegisterAsync(RegisterUserDTO dto);

    // Метод для аутентификации пользователя и получения токена
    Task<string> LoginAsync(LoginUserDTO dto);

    // Метод для получения информации о пользователе по его идентификатору
    Task<ReadForAccountUserDTO> ReadAsync(Guid id);
}
