using ChatestDomain.BusinessEntities.DTOs;

namespace ChatestDomain.BusinessLogic;

public interface IUserRepo
{
    Task RegisterAsync(RegisterUserDTO dto);
    Task<string> LoginAsync(LoginUserDTO dto);
    Task<ReadForAccountUserDTO> ReadAsync(Guid id);
}
